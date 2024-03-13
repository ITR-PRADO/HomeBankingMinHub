using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientService _clientService;
        private IAccountService _accountServices;
        private ICardService _cardService;
        public ClientsController(ICardService cardService, IAccountService accountServices, IClientService clientService)
        {
            _clientService = clientService;
            _accountServices = accountServices;
            _cardService = cardService;
        }
        
        [Authorize(policy: "AdminOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {                
                return Ok(_clientService.GetAllClients());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [Authorize(policy: "AdminOnly")]
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {              
                return Ok(_clientService.GetClientById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        

        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("ADMIN") != null ?
                User.FindFirst("ADMIN").Value : string.Empty;

                if (email == string.Empty)
                {
                    email = User.FindFirst("Client") != null ?
                                    User.FindFirst("Client").Value : string.Empty;
                    if (email == string.Empty)
                    {
                        return Forbid();
                    }                   
                }
                return Ok(_clientService.GetClientByEmail(email));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public IActionResult Post([FromBody] ClientSingUpDTO client)
        {
            try
            {
                if(String.IsNullOrEmpty(client.Email)) return StatusCode(400, "Email field empty, verify the information");
                if(String.IsNullOrEmpty(client.Password)) return StatusCode(400, "Password field empty, verify the information");
                if(String.IsNullOrEmpty(client.FirstName)) return StatusCode(400, "FirstName field empty, verify the information");
                if(String.IsNullOrEmpty(client.LastName)) return StatusCode(400, "LastName field empty, verify the information");
                
                
                return Created("User Successfully Created", _clientService.PostClient(new ClientDTO(client)));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpGet("current/accounts")]
        public IActionResult GetCurrentAccounts()
        {
            try
            {
                var idUser = User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty;
                if (idUser == String.Empty || !long.TryParse(idUser, out long idUserValue))
                {
                    return Forbid();
                }
                else
                {
                    return Ok(_accountServices.GetCurrentAccounts(idUserValue));
                }
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpGet("current/cards")]
        public IActionResult GetCurrentCards() {
            try
            {
                var idUser = User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty;
                if (idUser == String.Empty || !long.TryParse(idUser, out long idUserValue))
                {
                    return Forbid();
                }
                else
                {
                    return Ok(_cardService.GetCardsByClient(idUserValue));
                }
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpPost("current/accounts")]
        public IActionResult PostCurrentAccount()
        {
            try
            {
                var idUser = User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty;
                if (idUser == String.Empty || !long.TryParse(idUser, out long idUserValue))
                {
                    return Forbid();
                }
                else
                {
                    return Ok(_accountServices.PostAccount(idUserValue));
                }
                
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }


        //[Authorize(policy: "ClientOnly")]
        [Authorize]
        [HttpPost("current/cards")]
        public IActionResult PostCurrentCard(CardDTORquest cardParam)
        {
            try { 
            
                var idUser = User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty;
                if (idUser == String.Empty || !long.TryParse(idUser, out long idUserValue))
                {
                    return Forbid();
                }
                else
                {
                    var client = _clientService.GetClientById(idUserValue);
                    return Ok(_cardService.PostCard(client, cardParam));
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
