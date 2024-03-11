using HomeBankingMinHub.Models;
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
            Accounts = client.Accounts.Any() ? client.Accounts.Select(account => new AccountDTO(account)).ToList() : new List<AccountDTO>();
            Cards = client.Cards.Any() ? client.Cards.Select(card => new CardDTO(card)).ToList(): new List<CardDTO>();
            Credits =client.ClientLoans.Any() ? client.ClientLoans.Select(clientLoan => new ClientLoanDTO(clientLoan)).ToList() : new List<ClientLoanDTO>();
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
