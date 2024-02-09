namespace BnpParibas.Infrastructure
{
    public class ExternalDataProvider : IExternalDataProvider
    {
        public decimal GetPrice(string isin)
        {
            Random random = new Random();
            double d = random.NextDouble();
            return Convert.ToDecimal(d);
        }
    }
}
