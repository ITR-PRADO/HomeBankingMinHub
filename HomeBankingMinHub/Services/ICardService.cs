using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Services
{
    public interface ICardService
    {
        List<CardDTO> GetCardsByClient(long id);
        CardDTO PostCard(ClientDTO client, CardDTORquest cardParam);
    }
}
