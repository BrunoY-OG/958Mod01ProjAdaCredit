using AdaCredit.Logical.Services;

namespace AdaCredit.ConsoleUI.Display {
    public static class TransactionsMenuFunctions {
        public static TransactionService transactionService;

        public static void ProcessTransactions() =>
            transactionService.ProcessTransactions(ClientMenuFunctions.clientService);


        public static void ReportFailedTransactions() =>
            TablePrinter.PrintFailedTransactions(transactionService.FailedTransactionsReport());
    }
}
