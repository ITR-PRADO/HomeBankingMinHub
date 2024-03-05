using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAllLoan();
        void Save(Loan loan);
        Loan FindById(long id);
    }
}
