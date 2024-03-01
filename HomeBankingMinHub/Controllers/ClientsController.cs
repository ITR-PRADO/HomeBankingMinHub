using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICardRepository _cardRepository;
        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
        }
        
        [Authorize(policy: "AdminOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clients = _clientRepository.GetAllClients();
                var clientsDTO = new List<ClientDTO>();

                foreach (Client client in clients)
                {
                    var newClientDTO = new ClientDTO
                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Accounts = client.Accounts.Select(ac => new AccountDTO
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number
                        }).ToList(),
                        Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                        {
                            Id = cl.Id,
                            LoanId = cl.LoanId,
                            Name = cl.Loan.Name,
                            Amount = cl.Amount,
                            Payments = int.Parse(cl.Payments)
                        }).ToList(),
                        Cards = client.Cards.Select(c => new CardDTO
                        {
                            Id = c.Id,
                            CardHolder = c.CardHolder,
                            Color = c.Color.ToString(),
                            Cvv = c.Cvv,
                            FromDate = c.FromDate,
                            Number = c.Number,
                            ThruDate = c.ThruDate,
                            Type = c.Type.ToString()
                        }).ToList()
                    };                    
                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);
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
                var client = _clientRepository.FindById(id);
                if (client == null)
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type.ToString()
                    }).ToList()
                };

                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        
        [Authorize(policy: "ClientOnly")]
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? 
                    User.FindFirst("Client").Value : string.Empty;
                if(email == string.Empty)
                {
                    return Forbid();
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return Forbid();
                }
                var ClientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type.ToString()
                    }).ToList()
                };
                return Ok(ClientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClientDTO client)
        {
            try
            {
                if(String.IsNullOrEmpty(client.Email)) return StatusCode(403, "Email vacío");
                if(String.IsNullOrEmpty(client.Password)) return StatusCode(403, "Password vacía");
                if(String.IsNullOrEmpty(client.FirstName)) return StatusCode(403, "FirstName vacío");
                if(String.IsNullOrEmpty(client.LastName)) return StatusCode(403, "LastName vacío");
                
                Client user = _clientRepository.FindByEmail(client.Email);
                if(user != null)
                {
                    return StatusCode(403, "El Email esta en uso");
                }                
                string numberAccount;
                do
                {
                    numberAccount = Number.GenerateAccountNumber();
                } while (_accountRepository.Exist(numberAccount));
                List<Account> accounts = new List<Account>();
                accounts.Add(new Account
                {
                    Number = numberAccount,
                    CreationDate = DateTime.Now,
                    Balance = 0
                });
                Client newClient = new Client
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Rol = client.Rol,
                    Accounts= accounts
                };              
                _clientRepository.Save(newClient);
                return Created("", newClient);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [Authorize(policy:"ClientOnly")]
        [HttpGet("current/accounts")]
        public IActionResult GetCurrentAccounts()
        {
            var idUser = long.Parse(User.FindFirst("IdClient") != null ?
                User.FindFirst("IdClient").Value : string.Empty);
            var accounts = _accountRepository.GetAccountsByClient(idUser);
            List<AccountDTO> accountDTOs = new List<AccountDTO>();
            foreach(Account account in accounts)
            {
                AccountDTO accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(trans => new TransactionDTO
                    {
                        Id=trans.Id,
                        Type = trans.Type,
                        Amount = trans.Amount,
                        Description = trans.Description,
                        Date = trans.Date,
                    }).ToList(),

                };
                accountDTOs.Add(accountDTO);
            }               
                return Ok(accountDTOs);           
        }
        
        [Authorize(policy:"ClientOnly")]
        [HttpGet("current/cards")]
        public IActionResult GetCurrentCards() { 
            var idUser = long.Parse(User.FindFirst("IdClient") != null ?
                User.FindFirst("IdClient").Value : string.Empty);
            var Cards = _cardRepository.GetCardsByClient(idUser);
            List<CardDTO> cardDTOs = new List<CardDTO>();
            foreach(Card card in Cards)
            {
                var cardDto = new CardDTO
                {
                    Id = card.Id,
                    Number = card.Number,
                    Type = card.Type.ToString(),
                    Color = card.Color.ToString(),
                    FromDate = card.FromDate,
                    ThruDate = card.ThruDate,
                    CardHolder = card.CardHolder,
                    Cvv = card.Cvv,
                };
                cardDTOs.Add(cardDto);
            }
            return Ok(cardDTOs);
        }             

        [Authorize(policy:"ClientOnly")]
        [HttpPost("current/accounts")]
        public IActionResult PostCurrentAccount()
        {
            try
            {
                var idUser = long.Parse(User.FindFirst("IdClient") != null ?
                    User.FindFirst("IdClient").Value : string.Empty);
                if (_accountRepository.GetAccountsByClient(idUser).Count() >= 3)
                {
                    return StatusCode(403, "El Cliente ya posee su maximo de 3 cuentas");
                }
                else
                {
                    string numberAccount;
                    do
                    {
                        numberAccount = Number.GenerateAccountNumber();
                    } while (_accountRepository.Exist(numberAccount));
                    var newAccount = new Account
                    {
                        Number = numberAccount,
                        CreationDate = DateTime.Now,
                        Balance = 0,
                        ClientId = idUser
                    };
                    _accountRepository.Save(newAccount);
                    return Created("", newAccount);
                }
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        
        [Authorize(policy:"ClientOnly")]
        [HttpPost("current/cards")]
        public IActionResult PostCurrentCard(CardDTORquest cardParam)
        {
            try { 
            CardType cardType = (CardType)Enum.Parse(typeof(CardType), cardParam.Type);
            CardColor cardColor = (CardColor)Enum.Parse(typeof(CardColor), cardParam.Color);
            var idUser = long.Parse(User.FindFirst("IdClient") != null ?
                User.FindFirst("IdClient").Value : string.Empty);
            List<Card> cards = _cardRepository.GetCardsByClient(idUser).ToList();
            if(cards.Count() >= 6) return StatusCode(403, "El Cliente ya posee su maximo de 6 tarjetas");
            foreach(Card card in cards)
            {
                if (card.Type.Equals(cardType) && card.Color.Equals(cardColor))
                {
                    return StatusCode(403, "El Cliente ya posee una tarjeta de "+cardParam.Type+" de color "+cardParam.Color);
                }
            }
            string numberCard="";
            do
            {
                numberCard = Number.GenerateCreditNumber();
            }while(_cardRepository.Exist(numberCard));

            Client client = _clientRepository.FindById(idUser);
            var newCard = new Card
            {
               CardHolder= client.FirstName+" "+client.LastName,
               Type=cardType,
               Color=cardColor,
               ClientId=client.Id,
               Number=numberCard,
               Cvv=Number.GenerateCvv(),
               FromDate=DateTime.Now,
               ThruDate=DateTime.Now.AddYears(5),

            };
            _cardRepository.Save(newCard);
            return Created("", newCard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
