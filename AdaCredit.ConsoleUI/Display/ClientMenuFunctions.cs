using AdaCredit.Logical.Entities;
using AdaCredit.Logical.Services;
using BetterConsoles.Colors.Extensions;
using ConsoleTools;
using System.Drawing;

namespace AdaCredit.ConsoleUI.Display {
    public static class ClientMenuFunctions {
        public static ClientService clientService;

        public static Client? SelectClient() {
            if (!MainConsole.IsSetup) return null;
            Client? result = null;
            var clientSelector = Menu.SelectMenu("AdaCredit / Funcionários / Selecionar");
            List<Client> clients = clientService.SafeClientsList;
            foreach (Client client in clients) {
                clientSelector.Add($"{client.Name}: {client.Document}", () => {
                    result = client;
                    clientSelector.CloseMenu();
                });
            }
            clientSelector.Show();
            return result;
        }

        public static void ShowClientInfos() {
            if (!MainConsole.IsSetup) return;
            Client? client = SelectClient();
            if (client is null) return;
            TablePrinter.PrintClients(new List<Client> { client });
        }
        public static void ChangeName() {
            if (!MainConsole.IsSetup) return;
            Client? client = SelectClient();
            if (client is null) return;
            while (!clientService.ChangeClientName(client.Document, InputReceiver.AskNewName("clientes"))) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar nome, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }


        public static void ChangeEmail() {
            if (!MainConsole.IsSetup) return;
            Client? client = SelectClient();
            if (client is null) return;
            while (!clientService.ChangeClientName(client.Document, InputReceiver.AskNewName("clientes"))) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar nome, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }

        public static void ChangeAddress() {
            if (!MainConsole.IsSetup) return;
            Client? client = SelectClient();
            if (client is null) return;
            while (!clientService.ChangeClientName(client.Document, InputReceiver.AskNewName("clientes"))) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar nome, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }

        public static void Deactivate() {
            if (!MainConsole.IsSetup) return;
            Client? client = SelectClient();
            if (client is null) return;
            clientService.SetClientActive(client.Document, false); ;
        }
        public static void CreateNewClient() {
            if (!MainConsole.IsSetup) return;
            long document;
            do {
                document = InputReceiver.AskDocument();
            }
            while (!clientService.IsDocumentUnique(document));
            var name = InputReceiver.AskNewName("clientes");
            var email = InputReceiver.AskNewEmail("clientes");
            var address = InputReceiver.AskAddress();
            if (!clientService.AddNewClient(document, name, email, address)) {
                Console.WriteLine("Não foi possível cadastrar cliente.");
                ConsoleUtils.AwaitForKey();
            }
        }

        public static void ReportInactives() =>
            TablePrinter.PrintInactiveClients(clientService.ClientsReport(false));

        public static void ReportActives() =>
            TablePrinter.PrintClients(clientService.ClientsReport(true));
    }
}
