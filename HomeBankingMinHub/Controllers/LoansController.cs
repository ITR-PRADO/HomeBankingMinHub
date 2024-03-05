using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ILoanRepository _loanRepository;
        private IClientLoanRepository _clientLoanRepository;

        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository, ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
        }

        [Authorize("ClientOnly")]
        [HttpPost]
        public IActionResult PostLoan([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    //validar campos vacios, nulos o con valor 0
                    if (loanApplicationDTO.LoanId <= 0) return StatusCode(403, "Datos incompletos...(Credito)");
                    if (loanApplicationDTO.Payments.IsNullOrEmpty() || loanApplicationDTO.Payments == "0") return StatusCode(403, "Datos incompletos...(Pagos)");
                    if (loanApplicationDTO.ToAccountNumber.IsNullOrEmpty()) return StatusCode(403, "Datos incompletos...(Cuenta)");
                    //por medio de los claims busco el id del usuario loggeado y consultao su información
                    var userId = User.FindFirst("IdClient") != null ?
                                 User.FindFirst("IdClient").Value : String.Empty;
                    var client = _clientRepository.FindById(long.Parse(userId));
                    //Verifico que exista el prestamo
                    var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);
                    if (loan == null) return StatusCode(403, "El prestamo no existe");
                    // valido que el monto no sea negativo o igual a 0, ni supere el monto maximo
                    if (loanApplicationDTO.Amount <= 0 || loanApplicationDTO.Amount>loan.MaxAmount) return StatusCode(400, "Monto Incorrecto");
                    // creo un array de payments para poder recorrerlo con split() y valido si el valor ingresados e encuentra en el mismo
                    string[] payments = loan.Payments.Split(",");
                    if(payments.FirstOrDefault(a => a.Equals(loanApplicationDTO.Payments)) == null) return StatusCode(400, "Pagos Incorrectos");
                    //Verifico si la cuenta existe y luego si pertenece al cliente loggeado
                    var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);
                    if (account == null) return StatusCode(404, "Cuenta Inexistente");
                    if (client.Accounts.FirstOrDefault(account=>account.Number.Equals(loanApplicationDTO.ToAccountNumber))==null) return StatusCode(404, "La cuenta no pertenece al cliente");
                    //Se crea una solicitud de prestamo
                    var newClientLoan = new ClientLoan
                    {
                        Amount = loanApplicationDTO.Amount * 1.2,
                        Payments = loanApplicationDTO.Payments,
                        ClientId = long.Parse(userId),
                        LoanId = loanApplicationDTO.LoanId,
                    };
                    _clientLoanRepository.Save(newClientLoan);
                    var newTransaction = new Models.Transaction
                    {
                        Type = TransactionType.DEBIT,
                        Amount = loanApplicationDTO.Amount,
                        Description = "Loan approved",
                        Date = DateTime.Now,
                        AccountId = account.Id,
                    };
                    account.Transactions.Add(newTransaction);
                    account.Balance += loanApplicationDTO.Amount;
                    _accountRepository.Save(account);
                    scope.Complete();
                    return Created("", newClientLoan);
                }
                catch (Exception ex)
                {
                    return StatusCode(500,ex.Message);
                }

            }

        }

        
        [HttpGet]
        public IActionResult GetLoans()
        {
            return Ok(_loanRepository.GetAllLoan());
        }
    }
}
