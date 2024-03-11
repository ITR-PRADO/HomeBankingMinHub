using HomeBankingMinHub.Dtos;

namespace HomeBankingMinHub.Services
{
    public interface IClientService
    {
        List<ClientDTO> GetAllClients();
        ClientDTO GetClientById(long id);
        ClientDTO GetClientByEmail(string email);
        ClientDTO PostClient(ClientDTO client);
    }
}
