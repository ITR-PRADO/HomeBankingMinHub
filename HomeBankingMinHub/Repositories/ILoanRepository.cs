using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAllLoans();
        void Save(Loan loan);
        Loan FindById(long id);
    }
}
