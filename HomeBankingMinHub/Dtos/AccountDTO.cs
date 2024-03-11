using HomeBankingMinHub.Models;
using System.Text.Json.Serialization;

namespace HomeBankingMinHub.Dtos
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        [JsonIgnore]
        public long ClientId { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }

        public AccountDTO() { }
        public AccountDTO(Account account)
        {
            Id = account.Id;
            Number = account.Number;
            CreationDate = account.CreationDate;
            Balance = account.Balance;
            ClientId = account.ClientId;
            Transactions = account.Transactions.Any() ? account.Transactions.Select(transaction => new TransactionDTO(transaction)).ToList() : new List<TransactionDTO>(); ;
        }

    }
}
