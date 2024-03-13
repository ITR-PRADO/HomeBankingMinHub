using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IClientService _clientService;
        private IAccountService _accountService;
        private ILoanService _loanService;
        private IClientLoanService _clientLoanService;

        public LoansController(IClientService clientService, IAccountService accountService, ILoanService loanService, IClientLoanService clientLoanService)
        {
            _clientService = clientService;
            _accountService = accountService;
            _loanService = loanService;
            _clientLoanService = clientLoanService;
        }

        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpPost]
        public IActionResult PostLoan([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (loanApplicationDTO.LoanId <= 0) return StatusCode(400, "Credit field empty, verify the information");
                    if (loanApplicationDTO.Payments.IsNullOrEmpty() || loanApplicationDTO.Payments == "0") return StatusCode(400, "Payments field empty, verify the information");
                    if (loanApplicationDTO.ToAccountNumber.IsNullOrEmpty()) return StatusCode(400, "Accounts field empty, verify the information");
                    var userId = User.FindFirst("IdClient") != null ?
                                 User.FindFirst("IdClient").Value : String.Empty;
                    if (userId == String.Empty || !long.TryParse(userId, out long userIdValue)) return StatusCode(403, "Authentication Error");
                    var client = _clientService.GetClientById(userIdValue);                    
                    var loan = _loanService.GetLoanById(loanApplicationDTO.LoanId);

                    if (loan == null) return StatusCode(404, "Non-Existed Loan");
                    if (loanApplicationDTO.Amount <= 0 || loanApplicationDTO.Amount>loan.MaxAmount) return StatusCode(400, "Incorrect Amount");
                    string[] payments = loan.Payments.Split(",");
                    if(payments.FirstOrDefault(a => a.Equals(loanApplicationDTO.Payments)) == null) return StatusCode(400, "Incorrect Payment");
                    
                    var account = _accountService.GetAccountByNumber(loanApplicationDTO.ToAccountNumber);
                    if (account == null) return StatusCode(404, "Non-Existed Account");
                    if (client.Accounts.FirstOrDefault(account=>account.Number.Equals(loanApplicationDTO.ToAccountNumber))==null) return StatusCode(403, "The account does not belong to the client");
                    
                    
                    var clientLoanDTO = _clientLoanService.PostClientLoan(loanApplicationDTO, loan, userIdValue);
                    _accountService.PutAccountTransaction(loanApplicationDTO,account.Id, loan);                    
                    scope.Complete();

                    return Created("",clientLoanDTO);
                }
                catch (Exception ex)
                {
                    return StatusCode(500,ex.Message);
                }

            }

        }



        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpGet]
        public IActionResult GetLoans()
        {
            return Ok(_loanService.GetAllLoans());
        }
    }
}
