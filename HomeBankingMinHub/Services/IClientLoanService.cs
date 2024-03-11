using HomeBankingMinHub.Dtos;

namespace HomeBankingMinHub.Services
{
    public interface IClientLoanService
    {
        ClientLoanDTO PostClientLoan(LoanApplicationDTO loanApplicationDTO, LoanDTO loan, long userIdValue);
    }
}
