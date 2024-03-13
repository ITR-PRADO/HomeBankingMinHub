using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utilities;

namespace HomeBankingMinHub.Services.Impl
{
    public class CardService : ICardService
    {
        private ICardRepository _cardRepository;
        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        public List<CardDTO> GetCardsByClient(long id)
        {
            var Cards = _cardRepository.GetCardsByClient(id);
            List<CardDTO> listCardDTO = new List<CardDTO>();
            foreach (Card card in Cards)
            {
                var cardDto = new CardDTO(card);
                listCardDTO.Add(cardDto);
            }
            return listCardDTO;

        }

        public CardDTO PostCard(ClientDTO client, CardDTORquest cardParam)
        {
            CardType cardType = (CardType)Enum.Parse(typeof(CardType), cardParam.Type);
            CardColor cardColor = (CardColor)Enum.Parse(typeof(CardColor), cardParam.Color);
            List<Card> cards = _cardRepository.GetCardsByClient(client.Id).ToList();

            if (cards.Count() >= 6)
            {
                throw new Exception("The Client already has a maximum of 6 cards");
            }
            foreach (Card card in cards)
            {
                if (card.Type.Equals(cardType) && card.Color.Equals(cardColor))
                {
                    throw new Exception("The Client already has a "+cardParam.Color+" "+cardParam.Type+" card");
                }
            }
            string numberCard = "";
            do
            {
                numberCard = Number.GenerateCreditNumber();
            } while (_cardRepository.Exist(numberCard));
            var newCard = new Card();
            newCard.CardHolder = client.FirstName + " " + client.LastName;
            newCard.Type = cardType;
            newCard.Color = cardColor;
            newCard.ClientId = client.Id;
            newCard.Number = numberCard;
            newCard.Cvv = Number.GenerateCvv();
            
            _cardRepository.Save(newCard);

            return new CardDTO(newCard);
        }
    }
}
