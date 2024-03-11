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


        [Authorize(policy: "ClientOnly")]
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

                if ( transferDTO.Amount<=0|| transferDTO.Description.IsNullOrEmpty()) return StatusCode(400,"Datos incompletos");

                if(transferDTO.FromAccountNumber.IsNullOrEmpty()||transferDTO.ToAccountNumber.IsNullOrEmpty()) return StatusCode(400, "Datos de numero de cuenta vacios");

                AccountDTO accountFrom = _accountService.GetAccountByNumber(transferDTO.FromAccountNumber);
                if (accountFrom == null) return StatusCode(404, "No existe la cuenta de origen");

                AccountDTO accountTo = _accountService.GetAccountByNumber(transferDTO.ToAccountNumber);
                if (accountTo == null) return StatusCode(404, "No existe la cuenta de destino");

                if (accountFrom.ClientId != idUserValue) return StatusCode(403, "Cuenta incorrecta");

                if (accountFrom.Balance < transferDTO.Amount) return StatusCode(403, "Cuenta sin fondos");

                if (transferDTO.FromAccountNumber.Equals(transferDTO.ToAccountNumber)) return StatusCode(403, "Las cuentas de origen y destino son la misma");
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

