using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories.Impl
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Transaction FindById(long id)
        {
            return FindByCondition(transaction => transaction.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Transaction> GetTransactionsByAccount(long accountId)
        {
            return FindByCondition(transaction => transaction.AccountId == accountId)
                .ToList();
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return FindAll()
                .ToList();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }

    }
}
