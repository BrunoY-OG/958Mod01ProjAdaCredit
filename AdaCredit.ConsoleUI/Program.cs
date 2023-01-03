using AdaCredit.ConsoleUI.Display;
using AdaCredit.Logical.Entities;
using AdaCredit.Logical.Services;
using AdaCredit.Logical;

namespace AdaCredit.ConsoleUI {
    public class MainConsole {

        private static JSONPersistence employeePersistence = 
                    new(FileHandler.PathDataSave, JSONPersistence.PeristanceType.Employee);
        private static JSONPersistence clientPersistence = 
                    new(FileHandler.PathDataSave, JSONPersistence.PeristanceType.Client);
        public static bool shutdown = false;
        public static bool IsSetup {
            get {
                var isEmployeeServiceUp = EmployeesMenuFunctions.employeeService != null;
                var isClientServiceUp = ClientMenuFunctions.clientService != null;
                var isTransactionServiceUp = TransactionsMenuFunctions.transactionService != null;
                return isEmployeeServiceUp && isClientServiceUp && isTransactionServiceUp;
            }
        }


        static void Main(string[] args) {
            FileHandler.Setup();
            EmployeesMenuFunctions.employeeService = new EmployeeService(employeePersistence);
            ClientMenuFunctions.clientService = new ClientService(clientPersistence);
            TransactionsMenuFunctions.transactionService = new TransactionService();
            while (!shutdown) {
                if (EmployeesMenuFunctions.Welcome())
                    Menu.Run();
            }


            //var test = "BankB-2-20221201.csv";
            //Console.WriteLine($"year {test.CSVExtractYear()}");
            //Console.WriteLine($"month {test.CSVExtractMonth()}");
            //Console.WriteLine($"day {test.CSVExtractDay()}");
            //Console.WriteLine($"Date {test.CSVExtractDateOnly()}");
        }
    }
}
