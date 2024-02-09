namespace BnpParibas.Infrastructure
{
    public interface IExternalDataProvider
    {
        decimal GetPrice(string isin);
    }
}
