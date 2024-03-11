using HomeBankingMinHub.Models;
using System.Text.Json.Serialization;

namespace HomeBankingMinHub.Dtos
{
    public class ClientLoanDTO
    {
        public long Id { get; set; }
        public long LoanId { get; set; }
        [JsonIgnore]
        public long ClientId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }
        
        public ClientLoanDTO() { }
        public ClientLoanDTO(ClientLoan clientLoan)
        {
            Id = clientLoan.Id;
            LoanId = clientLoan.LoanId;
            ClientId = clientLoan.ClientId;
            Amount = clientLoan.Amount;
            Payments = int.Parse(clientLoan.Payments);
            Name = clientLoan.Loan.Name;
        }
    }
}
