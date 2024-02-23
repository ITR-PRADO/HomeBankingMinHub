using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Authorize(policy: "Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var acounts = _accountRepository.GetAllAccounts();
                var acountsDTO = new List<AccountDTO>();

                foreach (Account account in acounts)
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(trans => new TransactionDTO
                        {
                            Id = trans.Id,
                            Type = trans.Type,
                            Amount = trans.Amount,
                            Description = trans.Description,
                            Date = trans.Date
                        }).ToList()
                    };
                    acountsDTO.Add(newAccountDTO);
                }
                return Ok(acountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(policy: "ClientOnly")]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {              
                var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return Forbid();
                }
                //llamo al claim donde guarde el id del cliente
                //para asegurar que la cuenta pertenece a el comparando valores
                var idUser = User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty;
                if (!String.Equals(account.ClientId.ToString(), idUser))
                {
                    //en caso de no coincidir devuelvo unauthorized
                    return Unauthorized();
                }

                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(trans => new TransactionDTO
                    {
                        Id = trans.Id,
                        Type = trans.Type,
                        Amount = trans.Amount,
                        Description = trans.Description,
                        Date = trans.Date
                    }).ToList()
                };

                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
