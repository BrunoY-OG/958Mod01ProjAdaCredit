using AdaCredit.Logical.Entities;
using AdaCredit.Logical.Repositories;

namespace AdaCredit.Logical.Services
{
    public sealed class TransactionService {
        private TransactionRepository Repository;
        public TransactionService() { 
            Repository = new TransactionRepository();
        }

        public bool ProcessCSVFile(ClientService clientService, string file) {
            if (!File.Exists(Path.Combine(FileHandler.PathToPending, file))) return false;
            try {
                Repository.PrepareTransactionFiles(file, out TransactionFile transactionFile,
                    out TransactionFile completedTransactionFile, out FailedTransactionFile failedTransactionFile);
                foreach (var line in transactionFile.transactions) {
                    FailedTransactionCSVLine.FailureReason reason =
                        clientService.ExecuteTransaction(line);
                    if (reason == FailedTransactionCSVLine.FailureReason.NotApplicable) {
                        completedTransactionFile.transactions.Add(line);
                    }
                    else {
                        failedTransactionFile.transactions.Add(new FailedTransactionCSVLine(line, reason));
                    }
                }
                Repository.FinishTransactionFiles(transactionFile, completedTransactionFile, failedTransactionFile);
            }
            catch {
                return false;
            }
            return true;
        }

        public string ProcessTransactions(ClientService clientService) {
            string[] csvFiles = Repository.PendingNames;
            foreach (string csvFile in csvFiles) {
                if (!ProcessCSVFile(clientService, csvFile))
                    return csvFile;
            }
            return string.Empty;
        }

        public List<FailedTransactionCSVLine> FailedTransactionsReport() {
            List<FailedTransactionCSVLine> result = new List<FailedTransactionCSVLine>();
            List<string> names = Repository.FailedNames.ToList();
            foreach (string name in names) {
                FailedTransactionFile file = TransactionRepository.NewFailedFileFromCSV(FileHandler.PathToFailed, name);
                foreach (var line in file.transactions) {
                    result.Add(new(line));
                }
            }
            return result;
        }

    }
}
