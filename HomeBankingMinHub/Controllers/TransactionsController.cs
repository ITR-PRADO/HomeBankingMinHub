using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;
        private HomeBankingContext _homeBankingContext;
        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository, HomeBankingContext homeBankingContext)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _homeBankingContext = homeBankingContext;
        }


        [Authorize(policy: "ClientOnly")]
        [HttpPost]
        public IActionResult PostTransaction([FromBody] TransferDTO transferDTO)
        {
            using (var transaction = _homeBankingContext.Database.BeginTransaction())
            {
                try
                {
                    var idUser = long.Parse(User.FindFirst("IdClient") != null ?
                        User.FindFirst("IdClient").Value : string.Empty);
                    if ( transferDTO.Amount==0|| transferDTO.Description.IsNullOrEmpty()) return Forbid("Datos incompletos");

                    if(transferDTO.FromAccountNumber.IsNullOrEmpty()||transferDTO.ToAccountNumber.IsNullOrEmpty()) return Forbid("Datos de numero de cuenta vacios");

                    Account accountFrom = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
                    if (accountFrom == null) return Forbid("No existe la cuenta de origen");

                    Account accountTo = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
                    if (accountTo == null) return Forbid("No existe la cuenta de destino");

                    if (accountFrom.ClientId != idUser) return Forbid("Cuenta incorrecta");

                    if (accountFrom.Balance < transferDTO.Amount) return Forbid("Cuenta sin fondos");

                    if (transferDTO.FromAccountNumber.Equals(transferDTO.ToAccountNumber)) return Forbid("Las cuentas de origen y destino son la misma");
                    
                    accountFrom.Balance -= transferDTO.Amount;
                    accountTo.Balance += transferDTO.Amount;

                    accountFrom.Transactions.Add(new Transaction
                    {
                        Type = TransactionType.DEBIT,
                        Amount = -transferDTO.Amount,
                        Description = transferDTO.Description,
                        Date = DateTime.Now,
                        AccountId = accountFrom.Id,
                    });

                    accountTo.Transactions.Add(new Transaction
                    {
                        Type = TransactionType.CREDIT,
                        Amount = transferDTO.Amount,
                        Description = transferDTO.Description,
                        Date = DateTime.Now,
                        AccountId = accountTo.Id,
                    });
                    _accountRepository.Save(accountTo);
                    _accountRepository.Save(accountFrom);

                    transaction.Commit();
                    return Created();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

    }
}

