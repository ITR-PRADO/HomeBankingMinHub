using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private IAccountService _accountService;
        public TransactionsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpPost]
        public IActionResult PostTransaction([FromBody] TransferDTO transferDTO)
        {
            try
            {
                var idUser = User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty;
                if(idUser == String.Empty || !long.TryParse(idUser,out long idUserValue))
                {
                    return Forbid();
                }

                if ( transferDTO.Amount<=0|| transferDTO.Description.IsNullOrEmpty()) return StatusCode(400, "Transfer field empty, verify the information");

                if(transferDTO.FromAccountNumber.IsNullOrEmpty()||transferDTO.ToAccountNumber.IsNullOrEmpty()) return StatusCode(400, "Accounts field empty, verify the information");

                AccountDTO accountFrom = _accountService.GetAccountByNumber(transferDTO.FromAccountNumber);
                if (accountFrom == null) return StatusCode(404, "The source account does not exist");

                AccountDTO accountTo = _accountService.GetAccountByNumber(transferDTO.ToAccountNumber);
                if (accountTo == null) return StatusCode(404, "Destination account does not exist");

                if (accountFrom.ClientId != idUserValue) return StatusCode(403, "incorrect account");

                if (accountFrom.Balance < transferDTO.Amount) return StatusCode(403, "Insufficient fonts");

                if (transferDTO.FromAccountNumber.Equals(transferDTO.ToAccountNumber)) return StatusCode(403, "The source and destination accounts are the same");
                _accountService.SetTransaction(accountFrom, accountTo,transferDTO);
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            
        }

    }
}

