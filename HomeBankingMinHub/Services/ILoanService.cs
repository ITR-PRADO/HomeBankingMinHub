using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Services
{
    public interface ILoanService
    {
        List<LoanDTO> GetAllLoans();
        LoanDTO GetLoanById(long loanId);
    }
}
