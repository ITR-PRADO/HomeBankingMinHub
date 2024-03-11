using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
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

        [Authorize("ClientOnly")]
        [HttpPost]
        public IActionResult PostLoan([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (loanApplicationDTO.LoanId <= 0) return StatusCode(400, "Datos incompletos...(Credito)");
                    if (loanApplicationDTO.Payments.IsNullOrEmpty() || loanApplicationDTO.Payments == "0") return StatusCode(400, "Datos incompletos...(Pagos)");
                    if (loanApplicationDTO.ToAccountNumber.IsNullOrEmpty()) return StatusCode(400, "Datos incompletos...(Cuenta)");
                    var userId = User.FindFirst("IdClient") != null ?
                                 User.FindFirst("IdClient").Value : String.Empty;
                    if (userId == String.Empty || !long.TryParse(userId, out long userIdValue)) return StatusCode(403, "Error en autenticacion");
                    var client = _clientService.GetClientById(userIdValue);                    
                    var loan = _loanService.GetLoanById(loanApplicationDTO.LoanId);

                    if (loan == null) return StatusCode(404, "El prestamo no existe");
                    if (loanApplicationDTO.Amount <= 0 || loanApplicationDTO.Amount>loan.MaxAmount) return StatusCode(400, "Monto Incorrecto");
                    string[] payments = loan.Payments.Split(",");
                    if(payments.FirstOrDefault(a => a.Equals(loanApplicationDTO.Payments)) == null) return StatusCode(400, "Pagos Incorrectos");
                    
                    var account = _accountService.GetAccountByNumber(loanApplicationDTO.ToAccountNumber);
                    if (account == null) return StatusCode(404, "Cuenta Inexistente");
                    if (client.Accounts.FirstOrDefault(account=>account.Number.Equals(loanApplicationDTO.ToAccountNumber))==null) return StatusCode(403, "La cuenta no pertenece al cliente");
                    
                    
                    var clientLoanDTO = _clientLoanService.PostClientLoan(loanApplicationDTO, loan, userIdValue);
                    _accountService.PutAccountTransaction(loanApplicationDTO,account.Id);                    
                    scope.Complete();

                    return Created("",clientLoanDTO);
                }
                catch (Exception ex)
                {
                    return StatusCode(500,ex.Message);
                }

            }

        }



        [Authorize]
        [HttpGet]
        public IActionResult GetLoans()
        {
            return Ok(_loanService.GetAllLoans());
        }
    }
}
