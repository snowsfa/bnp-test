using BnpParibas.Domain;
using BnpParibas.Infrastructure;
using BnpParibas.Services;
using Moq;
using Serilog;

namespace BnpParibas.Tests
{
    public class SecurityServiceTests
    {
        private readonly ISecurityService _securityService;
        private readonly Mock<ISecurityRepository> _securityRepository = new();
        private readonly Mock<IExternalDataProvider> _dataProvider = new();
        private readonly Mock<ILogger> _logger = new();

        public SecurityServiceTests()
        {
            _securityService = new SecurityService(_securityRepository.Object, _dataProvider.Object, _logger.Object);
        }

        [Fact]
        public void UpdatePrices_WithNullArgument_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _securityService.UpdatePrices(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123ABC4567989")]
        public void UpdatePrices_WithInvalidIsin_LogsError(string isin)
        {
            HashSet<string> isins = new() { isin };
            _securityService.UpdatePrices(isins);
            _logger.Verify(d => d.Warning($"Invalid ISIN '{isin}'"), Times.Once);
            _dataProvider.Verify(d => d.GetPrice(isin), Times.Never);
            _securityRepository.Verify(d => d.GetByIsin(isin), Times.Never);
            _securityRepository.Verify(d => d.Add(It.IsAny<Security>()), Times.Never);
            _securityRepository.Verify(d => d.Update(It.IsAny<Security>()), Times.Never);
        }

        [Fact]
        public void UpdatePrices_WithValidArgument_PerformsWithSuccess()
        {
            const decimal price = 2564.99m;

            HashSet<string> isins = new()
            {
                "1234567890AB",
                "AB1234567890",
            };

            _securityService.UpdatePrices(isins);

            foreach (var isin in isins)
            {
                _dataProvider.Setup(d => d.GetPrice(isin)).Returns(price);
                Security security = new() { Isin = isin, Price = price };

                _securityRepository.Verify(r => r.GetByIsin(isin), Times.Once);
                _securityRepository.Verify(r => r.Update(security), Times.Once);
                _dataProvider.Verify(d => d.GetPrice(isin), Times.Once);
            }
        }

        [Fact]
        public void UpdatePrices_WhenErrorOnDataProviderOccours_LogsError()
        {
            HashSet<string> isins = new()
            {
                "1234567890AB",
                "AB1234567890",
            };

            _securityService.UpdatePrices(isins);
            _dataProvider.Setup(d => d.GetPrice(It.IsAny<string>())).Throws<Exception>();

            foreach (var isin in isins)
            {
                _securityRepository.Verify(r => r.GetByIsin(isin), Times.Never);
                _securityRepository.Verify(r => r.Add(It.IsAny<Security>()), Times.Never);
                _securityRepository.Verify(r => r.Update(It.IsAny<Security>()), Times.Never);
            }

        }
    }
}