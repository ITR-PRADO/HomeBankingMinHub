using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransactions();
        void Save(Transaction transaction);
        Transaction FindById(long id);
        IEnumerable<Transaction> GetTransactionsByAccount(long clientId);
    }
}
