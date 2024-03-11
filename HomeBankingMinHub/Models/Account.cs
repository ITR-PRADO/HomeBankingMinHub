using HomeBankingMinHub.Dtos;

namespace HomeBankingMinHub.Models
{
    public class Account
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public Client Client { get; set; }
        public long ClientId { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public Account()
        {
            CreationDate = DateTime.Now;
            Balance = 0;
            Transactions = new List<Transaction>();

        }

    }
}
