using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories
{
    public class CardRepository:RepositoryBase<Card>,ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public bool Exist(string numberCard)
        {
           return FindByCondition(card=>card.Number == numberCard).Any();
        }

        public Card FindById(long id)
        {
            return FindByCondition(card => card.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return FindByCondition(card => card.ClientId == clientId)
                .ToList();
        }

        public IEnumerable<Card> GetAllCard()
        {
            return FindAll()
                .ToList();                
        }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }

    }
}
