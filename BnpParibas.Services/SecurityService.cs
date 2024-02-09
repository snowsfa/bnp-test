using BnpParibas.Domain;
using BnpParibas.Infrastructure;
using Serilog;

namespace BnpParibas.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly IExternalDataProvider _dataProvider;
        private readonly ILogger _logger;
        const int _expectedIsinSize = 12;

        public SecurityService(ISecurityRepository securityRepository, IExternalDataProvider dataProvider, ILogger logger)
        {
            _securityRepository = securityRepository;
            _dataProvider = dataProvider;
            _logger = logger;
        }

        public void UpdatePrices(HashSet<string> isins)
        {
            if (isins == null)
            {
                throw new ArgumentNullException(nameof(isins));
            }

            foreach (string isin in isins)
            {
                if (isin == null || isin.Length != _expectedIsinSize)
                {
                    _logger.Warning($"Invalid ISIN '{isin}'");
                    continue;
                }

                try
                {
                    decimal? price = _dataProvider.GetPrice(isin);

                    if (price == null)
                    {
                        _logger.Warning($"ISIN '{isin}' not found on data provider");
                        continue;
                    }

                    Security security = _securityRepository.GetByIsin(isin);

                    if (security == null)
                    {
                        Security newSecurity = new()
                        {
                            Isin = isin,
                            Price = price.Value,
                        };

                        _securityRepository.Add(newSecurity);
                    }
                    else
                    {
                        if (price != security.Price)
                        {
                            security.Price = price.Value;
                            _securityRepository.Update(security);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error($"Error on getting ISIN price for '{isin}': {ex.Message}");
                }
            }
        }


    }
}