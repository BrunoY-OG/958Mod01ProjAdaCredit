using AdaCredit.Logical.Entities;

namespace AdaCredit.Logical.Repositories {
    public sealed class TransactionRepository {

        public string[] PendingNames { 
            get {
                return FileHandler.GetFileNames(FileHandler.PathToPending).Select(f => Path.GetFileName(f)).ToArray();
            }
        }
        public string[] CompletedNames {
            get {
                return FileHandler.GetFileNames(FileHandler.PathToCompleted).Select(f => Path.GetFileName(f)).ToArray();
            }
        }
        public string[] FailedNames {
            get {
                return FileHandler.GetFileNames(FileHandler.PathToFailed).Select(f => Path.GetFileName(f)).ToArray();
            }
        }

        public static TransactionFile NewFileFromCSV(string path, string name) {
            TransactionFile transactionFile = new();
            transactionFile.PathToFile = path;
            transactionFile.Name = name;
            transactionFile.FromCSV();
            return transactionFile;
        }

        public static FailedTransactionFile NewFailedFileFromCSV(string path, string name) {
            FailedTransactionFile transactionFile = new();
            transactionFile.PathToFile = path;
            transactionFile.Name = name;
            transactionFile.FromCSV();
            return transactionFile;
        }

        public bool PrepareTransactionFiles(string fileName, out TransactionFile? transaction, 
                    out TransactionFile? completedTransaction, out FailedTransactionFile? failedTransaction) {
            if (!File.Exists(Path.Combine(FileHandler.PathToPending, fileName)))  {
                transaction = null;
                completedTransaction = null;
                failedTransaction = null;
                return false; 
            }
            try {
                failedTransaction = FailedTransactionFile.NewFromCSV(FileHandler.PathToFailed,
                                                                                  fileName.CSVExtract() + "-failed.csv");
                completedTransaction = NewFileFromCSV(FileHandler.PathToCompleted,
                                                                                  fileName.CSVExtract() + "-completed.csv");
                transaction = NewFileFromCSV(FileHandler.PathToPending, fileName);
            }
            catch {
                transaction = null;
                completedTransaction = null;
                failedTransaction = null;
                return false;
            }
            return true;
        }

        public void FinishTransactionFiles(TransactionFile transaction,
                    TransactionFile completedTransaction, FailedTransactionFile failedTransaction) {
                if (completedTransaction.transactions.Count() > 0)
                    completedTransaction.ToCSV();
                if (failedTransaction.transactions.Count() > 0) 
                    failedTransaction.ToCSV();
            try {                
                File.Delete(Path.Combine(transaction.PathToFile, transaction.Name));
            }
            catch {
                throw;
            }
        }

    }
}
