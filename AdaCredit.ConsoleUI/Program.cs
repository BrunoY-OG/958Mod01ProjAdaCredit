using AdaCredit.ConsoleUI.Display;
using AdaCredit.Logical.Entities;
using AdaCredit.Logical.Services;

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
        }
    }
}
