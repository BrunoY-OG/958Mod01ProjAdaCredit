using AdaCredit.Logical.Bogus;
using ConsoleTools;

namespace AdaCredit.ConsoleUI.Display {
    internal class Menu {

        public static void Run() 
        {
            MainConsole.shutdown = false;

            var clientsRecordsMenu = new ConsoleMenu(Array.Empty<string>(), level: 2)
            .Add("Alterar nome de um cliente.", ClientMenuFunctions.ChangeName)
            .Add("Alterar email de um cliente.", ClientMenuFunctions.ChangeEmail)
            .Add("Alterar endereço de um cliente.", ClientMenuFunctions.ChangeAddress)
            .Add("Voltar", ConsoleMenu.Close)
            .Configure(ConsoleUtils.ConfigureMenu("Cadastro"));

            var clientsMenu = new ConsoleMenu(Array.Empty<string>(), level: 1)
            .Add("Cadastrar novo cliente.", ClientMenuFunctions.CreateNewClient)
            .Add("Consultar dados de um cliente.", ClientMenuFunctions.ShowClientInfos)
            .Add("Alterar o Cadastro de um Cliente.", clientsRecordsMenu.Show)
            .Add("Desativar cadastro de um cliente.", ClientMenuFunctions.Deactivate)
            .Add("Voltar", ConsoleMenu.Close)
            .Configure(ConsoleUtils.ConfigureMenu("Clientes"));

            var employeesMenu = new ConsoleMenu(Array.Empty<string>(), level: 1)
            .Add("Cadastrar novo funcionário.", EmployeesMenuFunctions.CreateNewUser)
            .Add("Alterar nome de um funcionário.", EmployeesMenuFunctions.ChangeName)
            .Add("Alterar senha de um funcionário.", EmployeesMenuFunctions.ChangePassword)
            .Add("Alterar apelido de um funcionário.", EmployeesMenuFunctions.ChangeUsername)
            .Add("Alterar email de um funcionário.", EmployeesMenuFunctions.ChangeEmail)
            .Add("Desativar cadastro de um funcionário.", EmployeesMenuFunctions.Deactivate)
            .Add("Voltar", ConsoleMenu.Close)
            .Configure(ConsoleUtils.ConfigureMenu("Funcionários"));

            var transactionsMenu = new ConsoleMenu(Array.Empty<string>(), level: 1)
            .Add("Processar Transações (Reconciliação Bancária).", (thisMenu) => {
                TransactionsMenuFunctions.ProcessTransactions();
                thisMenu.CurrentItem.Name = "Transações processadas";
            })
            .Add("Voltar", ConsoleMenu.Close)
            .Configure(ConsoleUtils.ConfigureMenu("Transações"));

            var reportsMenu = new ConsoleMenu(Array.Empty<string>(), level: 1)
            .Add("Exibir todos os clientes ativos com seus respectivos saldos.", ClientMenuFunctions.ReportActives)
            .Add("Exibir todos os clientes inativos.", ClientMenuFunctions.ReportInactives)
            .Add("Exibir todos os funcionários ativos e sua última data e hora de login.", EmployeesMenuFunctions.ReportActiveEmployees)
            .Add("Exibir Transações com Erro (Detalhes da transação e do Erro).", TransactionsMenuFunctions.ReportFailedTransactions)
            .Add("Voltar", ConsoleMenu.Close)
            .Configure(ConsoleUtils.ConfigureMenu("Relatórios"));

            var bogusMenu = new ConsoleMenu(Array.Empty<string>(), level: 1)
            .Add("Gerar clientes fantasmas.", (thisMenu) => 
            {
                BogusClient.AutoBuildUniqueClient();
                thisMenu.CurrentItem.Name = "Clientes gerados";
            })
            .Add("Gerar funcionários fantasmas.", (thisMenu) => 
            {
                BogusEmployee.GenerateEmployeeRepository();
                thisMenu.CurrentItem.Name = "Funcionários gerados";
            })
            .Add("Gerar arquivos de transação fantasmas.", (thisMenu) => {
                BogusFiles.GenerateCSV();
                thisMenu.CurrentItem.Name = "Arquivos gerados";
            })
            .Add("Voltar", ConsoleMenu.Close)
            .Configure(ConsoleUtils.ConfigureMenu("Relatórios"));

            var mainMenu = new ConsoleMenu(Array.Empty<string>(), level: 0)
              .Add("Clientes", clientsMenu.Show)
              .Add("Funcionários", employeesMenu.Show)
              .Add("Transações", transactionsMenu.Show)
              .Add("Relatórios", reportsMenu.Show)
              .Add("Sair da conta", ConsoleMenu.Close)
              .Add("Desligar", (thisMenu) => { 
                                                MainConsole.shutdown = true; 
                                                thisMenu.CloseMenu();
                                            })
              .Add("Geração automática de informações", bogusMenu.Show)
              .Configure(ConsoleUtils.ConfigureMenu("AdaCredit"));
            mainMenu.Show();

            return;
        }

        public static ConsoleMenu SelectMenu(string nome) 
        {
            return new ConsoleMenu(Array.Empty<string>(), level: 2)
            .Add("Voltar", ConsoleMenu.Close)
            .Configure(ConsoleUtils.ConfigureList(nome));
        }


    }
}
