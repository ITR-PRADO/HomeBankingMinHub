using HomeBankingMinHub.Dtos;

namespace HomeBankingMinHub.Services
{
    public interface IAccountService
    {
        List<AccountDTO> GetCurrentAccounts(long id);
        List<AccountDTO> GetAllAccounts();
        AccountDTO PostAccount(long id);
        AccountDTO GetAccountById(long id);
        AccountDTO GetAccountByNumber(string toAccountNumber);
        AccountDTO PutAccountTransaction(LoanApplicationDTO loanApplicationDTO,long id);
        void SetTransaction(AccountDTO accountFrom, AccountDTO accountTo, TransferDTO transferDTO);
    }
}
