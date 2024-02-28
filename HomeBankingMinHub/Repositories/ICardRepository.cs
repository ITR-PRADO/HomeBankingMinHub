using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCard();
        void Save(Card card);
        Card FindById(long id);
        IEnumerable<Card> GetCardsByClient(long clientId);
        bool Exist(string numberCard);
    }
}
