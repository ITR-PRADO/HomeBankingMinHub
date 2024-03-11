using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Dtos
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserDTO() { }
        public UserDTO(Client client)
        {
            Email = client.Email;
            Password = client.Password;
        }
    }
}
