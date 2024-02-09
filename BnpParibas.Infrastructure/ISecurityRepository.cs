using BnpParibas.Domain;

namespace BnpParibas.Infrastructure
{
    public interface ISecurityRepository
    {
        void Add(Security security);

        Security GetByIsin(string isin);

        void Update(Security security);
    }
}
