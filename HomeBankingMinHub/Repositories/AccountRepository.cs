using HomeBankingMinHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMinHub.Repositories
{
    public class AccountRepository:RepositoryBase<Account>,IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public bool Exist(string numberAccount)
        {
           return FindByCondition(account=>account.Number == numberAccount).Any();
        }

        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }

        public Account FindByNumber(string number)
        {
            return FindByCondition(account=>account.Number.ToUpper() == number.ToUpper())
                .Include(account => account.Transactions)
                .FirstOrDefault();

        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account=> account.ClientId == clientId)
                .Include(account=>account.Transactions)
                .ToList();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList();                
        }

        public void Save(Account account)
        {
            if (account.Id == 0)
            {
                Create(account);
            }
            else
            {
                Update(account);
            }
            SaveChanges();
            RepositoryContext.ChangeTracker.Clear();
        }


    }
}
