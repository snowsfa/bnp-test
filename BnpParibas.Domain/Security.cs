namespace BnpParibas.Domain
{
    public class Security
    {
        public string Isin { get; set; }

        public decimal Price { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Security security &&
                   Isin == security.Isin &&
                   Price == security.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Isin, Price);
        }
    }
}
