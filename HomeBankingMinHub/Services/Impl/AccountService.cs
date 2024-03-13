using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utilities;
using System.Transactions;

namespace HomeBankingMinHub.Services.Impl
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public AccountDTO GetAccountById(long id)
        {         
            return new AccountDTO(_accountRepository.FindById(id));
        }

        public AccountDTO GetAccountByNumber(string toAccountNumber)
        {
            Account account = _accountRepository.FindByNumber(toAccountNumber);
            if(account == null)
            {
                throw new Exception("Non-existent account");
            }
            return new AccountDTO(account);
        }

        public List<AccountDTO> GetAllAccounts()
        {
            var acounts = _accountRepository.GetAllAccounts();
            var listAccountDTO = new List<AccountDTO>();
            foreach (Account account in acounts)
            {
                var newAccountDTO = new AccountDTO(account);                
                listAccountDTO.Add(newAccountDTO);
            }
            return listAccountDTO;
        }

        public List<AccountDTO> GetCurrentAccounts(long id)
        {
            var accounts = _accountRepository.GetAccountsByClient(id);
            if (accounts == null)
            {
                throw new Exception("No se encontraron cuentas");
            }
            List<AccountDTO> listAccountDTO = new List<AccountDTO>();
            foreach (Account account in accounts)
            {
                AccountDTO accountDTO = new AccountDTO(account);
                listAccountDTO.Add(accountDTO);
            }
            return listAccountDTO;
        }

        public AccountDTO PostAccount(long id)
        {
            if (_accountRepository.GetAccountsByClient(id).Count() >= 3)
            {
                throw new Exception("The Client already has a maximum of 3 accounts");
            }
            else
            {
                string numberAccount;
                do
                {
                    numberAccount = Number.GenerateAccountNumber();
                } while (_accountRepository.Exist(numberAccount));
                var newAccount = new Account();
                newAccount.Number = numberAccount;
                newAccount.ClientId = id;
                _accountRepository.Save(newAccount);
                return new AccountDTO(newAccount);
            }
        }

        public AccountDTO PutAccountTransaction(LoanApplicationDTO loanApplicationDTO, long id, LoanDTO loan)
        {
            Account account = _accountRepository.FindById(id);
            var newTransaction = new Models.Transaction
            {
                Type = TransactionType.CREDIT,
                Amount = loanApplicationDTO.Amount,
                Description = "Loan "+ loan.Name+" approved",
                Date = DateTime.Now,
                AccountId = account.Id,
            };
            account.Transactions.Add(newTransaction);
            account.Balance += loanApplicationDTO.Amount;
            _accountRepository.Save(account);
            return new AccountDTO(account);
        }

        public void SetTransaction(AccountDTO accountDTOFrom, AccountDTO accountDTOTo, TransferDTO transferDTO)
        {
            using (var scope = new TransactionScope())
            {
                try
                {

                    Account accountFrom = _accountRepository.FindById(accountDTOFrom.Id);
                    Account accountTo = _accountRepository.FindById(accountDTOTo.Id);
                    accountFrom.Balance -= transferDTO.Amount;
                    accountTo.Balance += transferDTO.Amount;

                    accountFrom.Transactions.Add(new Models.Transaction
                    {
                        Type = TransactionType.DEBIT,
                        Amount = -transferDTO.Amount,
                        Description = transferDTO.Description,
                        Date = DateTime.Now,
                    });

                    accountTo.Transactions.Add(new Models.Transaction
                    {
                        Type = TransactionType.CREDIT,
                        Amount = transferDTO.Amount,
                        Description = transferDTO.Description,
                        Date = DateTime.Now,
                    });
                    _accountRepository.Save(accountTo);
                    _accountRepository.Save(accountFrom);
                    scope.Complete();
                }catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }


        }
    }
}
