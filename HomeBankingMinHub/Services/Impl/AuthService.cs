using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace HomeBankingMinHub.Services.Impl
{
    public class AuthService : IAuthService
    {
        private IClientRepository _clientRepository;
        public AuthService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public ClaimsIdentity SetClaims(ClientDTO clientDTO)
        {
            var claims = new List<Claim>
                {
                    new Claim(clientDTO.Rol.ToString(), clientDTO.Email),
                    new Claim("IdClient",clientDTO.Id.ToString()),
                    new Claim("NameClient",clientDTO.FirstName+" "+clientDTO.LastName),
                };

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
