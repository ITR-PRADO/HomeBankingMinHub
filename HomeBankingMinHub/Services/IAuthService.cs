using HomeBankingMinHub.Dtos;
using System.Security.Claims;

namespace HomeBankingMinHub.Services
{
    public interface IAuthService
    {
        ClaimsIdentity SetClaims(ClientDTO clientDTO);
    }
}
