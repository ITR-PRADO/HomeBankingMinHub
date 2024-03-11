using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;

namespace HomeBankingMinHub.Services.Impl
{
    public class ClientLoanService : IClientLoanService
    {
        private readonly IClientLoanRepository _clientLoanRepository;
        public ClientLoanService(IClientLoanRepository clientLoanRepository)
        {
            _clientLoanRepository = clientLoanRepository;
        }

        public ClientLoanDTO PostClientLoan(LoanApplicationDTO loanApplicationDTO, LoanDTO loan, long userIdValue)
        {
            var newClientLoan = new ClientLoan
            {
                Amount = loanApplicationDTO.Amount * 1.2,
                Payments = loanApplicationDTO.Payments,
                ClientId = userIdValue,
                LoanId = loanApplicationDTO.LoanId,
            };
            _clientLoanRepository.Save(newClientLoan);
            ClientLoanDTO clientLoanDTO = new ClientLoanDTO
            {
                Id = newClientLoan.Id,
                LoanId = newClientLoan.Id,
                ClientId = newClientLoan.Id,
                Name = loan.Name,
                Amount = newClientLoan.Amount,
                Payments = int.Parse(newClientLoan.Payments)

            };
            return clientLoanDTO;
        }
    }
}
