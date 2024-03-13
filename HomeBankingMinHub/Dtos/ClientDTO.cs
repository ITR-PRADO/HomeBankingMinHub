using HomeBankingMinHub.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace HomeBankingMinHub.Dtos
{
    public class ClientDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public Rol Rol { get; set; }
        public ICollection<AccountDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Credits { get; set; }
        public ICollection<CardDTO> Cards { get; set; }

        public ClientDTO() { }
        public ClientDTO(Client client)
        {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;
            Password = client.Password;
            Rol = client.Rol;
            Accounts = client.Accounts.IsNullOrEmpty() ? null : client.Accounts.Select(account => new AccountDTO(account)).ToList();
            Cards = client.Cards.IsNullOrEmpty() ? new List<CardDTO>() : client.Cards.Select(card => new CardDTO(card)).ToList();
            Credits =client.ClientLoans.IsNullOrEmpty() ? null : client.ClientLoans.Select(clientLoan => new ClientLoanDTO(clientLoan)).ToList();
        }
        public ClientDTO(ClientSingUpDTO client)
        {
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;
            Password = client.Password;
            Rol = Rol.CLIENT;
            Accounts = new List<AccountDTO>();
            Cards = new List<CardDTO>();
            Credits = new List<ClientLoanDTO>();
        }
    }
}
