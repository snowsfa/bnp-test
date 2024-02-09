using BnpParibas.Domain;
using Microsoft.EntityFrameworkCore;

namespace BnpParibas.Infrastructure
{
    public class SecurityRepository : ISecurityRepository
    {
        public readonly DbContext _dbContext;

        public SecurityRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Security security)
        {
            _dbContext.Add(security);
            _dbContext.SaveChanges();
        }

        public Security GetByIsin(string isin)
        {
            return new Security()
            {
                Isin = "ISIN456789AZ",
                Price = 25899.99m
            };
        }

        public void Update(Security security)
        {
            _dbContext.Update(security);
            _dbContext.SaveChanges();
        }
    }
}