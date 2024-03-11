using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;

namespace HomeBankingMinHub.Services.Impl
{
    public class LoanService : ILoanService
    {
        private ILoanRepository _loanRepository;
        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public List<LoanDTO> GetAllLoans()
        {
            List<Loan> listLoan = _loanRepository.GetAllLoans().ToList();
            List<LoanDTO> listLoanDTO = new List<LoanDTO>();
            foreach (Loan loan in listLoan)
            {
                listLoanDTO.Add(new LoanDTO(loan));
            }
            return listLoanDTO;
        }

        public LoanDTO GetLoanById(long loanId)
        {
            return new LoanDTO(_loanRepository.FindById(loanId));
        }
    }
}
