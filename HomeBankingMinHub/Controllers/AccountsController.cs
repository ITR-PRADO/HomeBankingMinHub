using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountsController(IAccountService accountServices)
        {
            _accountService = accountServices;
        }

        [Authorize(policy: "AdminOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {               
                return Ok(_accountService.GetAllAccounts());
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
                var accountDTO = _accountService.GetAccountById(id);
                if (accountDTO == null)
                {
                    return NotFound("La cuenta no existe");
                }

                var idUser = User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty;
                if (!String.Equals(accountDTO.ClientId.ToString(), idUser))
                {
                    //en caso de no coincidir devuelvo unauthorized
                    return Unauthorized();
                }
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
