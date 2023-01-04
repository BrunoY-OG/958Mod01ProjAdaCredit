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
                FileHandler.Setup();
                if (EmployeesMenuFunctions.employeeService is null)
                    EmployeesMenuFunctions.employeeService = new EmployeeService(employeePersistence);
                if (ClientMenuFunctions.clientService is null)
                    ClientMenuFunctions.clientService = new ClientService(clientPersistence);
                if (TransactionsMenuFunctions.transactionService is null)
                    TransactionsMenuFunctions.transactionService = new TransactionService();
                var isEmployeeServiceUp = EmployeesMenuFunctions.employeeService != null;
                var isClientServiceUp = ClientMenuFunctions.clientService != null;
                var isTransactionServiceUp = TransactionsMenuFunctions.transactionService != null;
                return isEmployeeServiceUp && isClientServiceUp && isTransactionServiceUp;
            }
        }


        static void Main(string[] args) {
            if (!IsSetup) return;
            while (!shutdown) {
                if (EmployeesMenuFunctions.Welcome())
                    Menu.Run();
            }
        }
    }
}
