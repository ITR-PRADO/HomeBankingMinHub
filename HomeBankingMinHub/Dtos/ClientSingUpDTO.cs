using HomeBankingMinHub.Models;
using System.Text.Json.Serialization;

namespace HomeBankingMinHub.Dtos
{
    public class ClientSingUpDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ClientSingUpDTO() { }
    }
}
