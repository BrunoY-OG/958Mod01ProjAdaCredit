using AdaCredit.Logical.Entities;
using AdaCredit.Logical.Repositories;
using Bogus;
using System.Security.Principal;

namespace AdaCredit.Logical.Services{
    public sealed class ClientService {
        private static readonly string PATH = Path.Combine(FileHandler.PathDataSave, "Clients.json");

        public const string AdaCreditBankCode = "777";
        public const string AdaCreditBranchCode = "0001";
        private ClientRepository Repository { get; init; }
        public ClientService(IDataPersistence dataPersistence) {
            Repository = new ClientRepository(dataPersistence);
        }
        public List<Client> SafeClientsList {
            get {
                Repository.LoadClienList();
                return Repository.Clients.Select(SafeClient).ToList();
            }
        }

        public List<Account> SafeAccountsList {
            get {
                Repository.LoadClienList();
                return Repository.Clients.Where(c => c.Account != null)
                    .Select(c => SafeAccount(c.Account))
                    .ToList();
            }
        }
        public Account SafeAccount(Account account) =>
            new Account {
                BankCode = account.BankCode,
                BranchCode = account.BranchCode,
                Number = account.Number,
                Balance = account.Balance,
                Active = account.Active
            };

        public Client SafeClient(Client client) =>
            new Client {
                Name = client.Name,
                Document = client.Document,
                Email = client.Email,
                Address = client.Address,
                CreatedAt = client.CreatedAt,
                Active = client.Active,
                Account = SafeAccount(client.Account)
            };

        public Account GenerateUniqueAccount() {
            Repository.LoadClienList();
            var faker = new Faker("pt_BR");
            string accNumber;
            do {
                accNumber = faker.Random.Replace("#####-#");
            } while (IsAccountNumberUnique(accNumber));
            return new Account {
                Active = true,
                Balance = 0,
                Number = accNumber,
                BankCode = AdaCreditBankCode,
                BranchCode = AdaCreditBranchCode,
            };
        }


        public bool IsDocumentUnique(long document) {
            Repository.LoadClienList();
            return Repository.GetClientByDocument(document) is null;
        }

        public bool IsAccountNumberUnique(string accNumber) {
            Repository.LoadClienList();
            return Repository.GetClientByAccountNumber(accNumber) is null;
        }

        public bool ChangeClientName(long document, string name) {
            Repository.LoadClienList();
            Client? client = Repository.GetClientByDocument(document);
            if (client != null) {
                client.Name = name;
                Repository.SaveClientList();
                return true;
            }
            return false;
        }

        public bool ChangeClientEmail(long document, string email) {
            Repository.LoadClienList();
            Client? client = Repository.GetClientByDocument(document);
            if (client != null) {
                client.Email = email;
                Repository.SaveClientList();
                return true;
            }
            return false;
        }

        public bool ChangeClientAddress(long document, string address) {
            Repository.LoadClienList();
            Client? client = Repository.GetClientByDocument(document);
            if (client != null) {
                client.Address = address;
                Repository.SaveClientList();
                return true;
            }
            return false;
        }


        public bool SetClientActive(long document, bool set) {
            Repository.LoadClienList();
            Client? client = Repository.GetClientByDocument(document);
            if (client != null) {
                client.Active = set;
                Repository.SaveClientList();
                return true;
            }
            return false;
        }

        private Account? GetAccount(string accountBank, string accountBranch, string accNumber) {
            Repository.LoadClienList();
            if (!string.Equals(accountBank, AdaCreditBankCode))
                return null;
            else if (!string.Equals(accountBranch, AdaCreditBranchCode))
                return null;
            var corr = accNumber;
            if (accNumber.IndexOf('-') == -1) corr = accNumber.ToggleAccountDash();
            return Repository.GetAccount(corr);
        }

        public Account? GetSafeAccount(string accountBank, string accountBranch, string accNumber) {
            Repository.LoadClienList();
            var account = GetAccount(accountBank, accountBranch, accNumber);
            if (account is null) return null;
            return SafeAccount(account);
        }

        public bool AddNewClient(long document, string name, string email, string address) {
            Repository.LoadClienList();
            if (!IsDocumentUnique(document)) return false;
            Repository.AddClient(new() 
            {
                Account = GenerateUniqueAccount(),
                Document = document,
                Name = name,
                Email = email,
                Address = address
            });
            return true;
        }

        public FailedTransactionCSVLine.FailureReason CheckTransaction(TransactionCSVLine transaction,
                        DateOnly dateOnly, 
                        out Account? destAccount, out Account? originAccount, out decimal feeValue) 
        {
            Repository.LoadClienList();
            destAccount = GetAccount(transaction.destBankCode, transaction.destBranchCode, transaction.destAccount);
            originAccount = GetAccount(transaction.originBankCode, transaction.originBranchCode, transaction.originAccount);
            feeValue = FeeCalculator.CalculateFee(dateOnly, transaction.value, transaction.transactionType);
            if ((destAccount is null) && (originAccount is null))
                return FailedTransactionCSVLine.FailureReason.InvalidAccount;
            else if ((destAccount != null) && (originAccount != null)) {
                if (transaction.transactionType != TransactionCSVLine.TransactionType.TEF) return FailedTransactionCSVLine.FailureReason.IncompatibleType;
                else if (originAccount.Balance < transaction.value + feeValue) return FailedTransactionCSVLine.FailureReason.InsuficientFunds;
            }
            else if ((destAccount == null) && (originAccount != null)) {
                if (transaction.direction == 1) return FailedTransactionCSVLine.FailureReason.WrongDirection;
                else if (originAccount.Balance < transaction.value + feeValue) return FailedTransactionCSVLine.FailureReason.InsuficientFunds;
            }
            else if ((destAccount != null) && (originAccount == null)) {
                if (transaction.direction == 0) return FailedTransactionCSVLine.FailureReason.WrongDirection;
            }
            else if ((destAccount == null) && (originAccount == null)) {
                return FailedTransactionCSVLine.FailureReason.NotPertinent;
            }
            return FailedTransactionCSVLine.FailureReason.NotApplicable;
        }

        public FailedTransactionCSVLine.FailureReason ExecuteTransaction(TransactionCSVLine transaction, DateOnly dateOnly) {
            FailedTransactionCSVLine.FailureReason failureReason = CheckTransaction(transaction, new DateOnly(2022, 11, 30),
                                                                    out Account destAccount, out Account originAccount, 
                                                                    out decimal feeValue);
            if (failureReason != FailedTransactionCSVLine.FailureReason.NotApplicable)
                return failureReason;
            else if ((destAccount != null) && (originAccount != null)) {
                destAccount.Balance += transaction.value;
                originAccount.Balance -= transaction.value + feeValue;
            }
            else if ((destAccount == null) && (originAccount != null)) {
                originAccount.Balance -= transaction.value + feeValue;
            }
            else if ((destAccount != null) && (originAccount == null)) {
                destAccount.Balance += transaction.value;
            }
            return FailedTransactionCSVLine.FailureReason.NotApplicable;
        }

        public List<Client> ClientsReport(bool active) =>
            SafeClientsList.Where(c => c.Active == active).ToList();
        

    }
}
