
using AdaCredit.Logical.Entities;

namespace AdaCredit.Logical.Repositories {
    public sealed class ClientRepository {
        private IDataPersistence dataPersistence;
        private List<Client> clients = new List<Client>();
        internal IEnumerable<Client> Clients { get => clients; }
        internal IEnumerable<Account> Accounts { get => clients.Select(c => c.Account); }

        internal ClientRepository(IDataPersistence persistence) {
            dataPersistence = persistence;
            LoadClienList();
        } 
        public void LoadClienList() {
            dataPersistence.LoadData(out clients);
        }
        public void SaveClientList() {
            dataPersistence.SaveData(clients);
        }

        public void AddClient(Client client) {
            clients.Add(client);
            SaveClientList();
        }

        public Client? GetClientByDocument(long documentId) =>
            clients.FirstOrDefault(c => c.Document == documentId);
        public Client? GetClientByAccountNumber(string accountNumber) =>
            clients.FirstOrDefault(c => c.Account.Number == accountNumber);
        public Account? GetAccount(string accountNumber) =>
            clients.FirstOrDefault(c => c.Account.Number == accountNumber)?.Account;

    }
}
