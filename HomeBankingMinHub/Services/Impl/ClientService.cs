using HomeBankingMinHub.Dtos;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories;
using HomeBankingMinHub.Utilities;

namespace HomeBankingMinHub.Services.Impl
{
    public class ClientService : IClientService
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }

        public List<ClientDTO> GetAllClients()
        {
           var clients = _clientRepository.GetAllClients();
           List<ClientDTO> clientDTOList = new List<ClientDTO>();
           foreach(var client in clients)
            {
                clientDTOList.Add(new ClientDTO(client));
            }
           return clientDTOList;
        }

        public ClientDTO GetClientByEmail(string email)
        {
            var client = _clientRepository.FindByEmail(email);
            if (client == null)
            {
                throw new Exception("Cliente no encontrado...");
            }
            return new ClientDTO(client);
        }

        public ClientDTO GetClientById(long id)
        {
            var client = _clientRepository.FindById(id);
            if (client == null)
            {
                throw new Exception("No se encontro al cliente...");
            }
            return new ClientDTO(client);
        }

        public ClientDTO PostClient(ClientDTO client)
        {
            var user = _clientRepository.FindByEmail(client.Email);
            if (user != null)
            {
                throw new Exception("El email ya esta en uso");
            }
            string numberAccount;
            do
            {
                numberAccount = Number.GenerateAccountNumber();
            } while (_accountRepository.Exist(numberAccount));
            List<Account> accounts = new List<Account>();
            accounts.Add(new Account
            {
                Number = numberAccount,
                CreationDate = DateTime.Now,
                Balance = 0,
                Transactions= new List<Transaction>()
                

            });
            Client newClient = new Client
            {
                Email = client.Email,
                Password = client.Password,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Rol = client.Rol,
                Accounts = accounts,
                Cards=new List<Card>(),
                ClientLoans = new List<ClientLoan>()
            };
            _clientRepository.Save(newClient);
            return new ClientDTO(newClient);
        }


    }
}
