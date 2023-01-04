using AdaCredit.Logical.Services;
using GetPass;

namespace AdaCredit.ConsoleUI.Display
{
    public static class InputReceiver {

        public static string AskNewEmail(string tipo) 
        {
            if (!MainConsole.IsSetup) return string.Empty;
            string? email;
            string? confirm;
            bool areEquals = true;
            do {
                Console.Clear();
                Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
                Console.WriteLine($"         Controle de {tipo}");
                if (!areEquals) Console.WriteLine("         Emails diferentes, tente novamente.");
                Console.Write("Forneça seu Email: ");
                email = Console.ReadLine();
                while (!Logical.StringUtils.IsValidEmail(email)) {
                    Console.Clear();
                    Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
                    Console.WriteLine($"         Controle de {tipo}");
                    Console.WriteLine("Email inválido.");
                    Console.Write("Forneça seu Email: ");
                    email = Console.ReadLine();
                }
                Console.Write("Confirme seu Email: ");
                confirm = Console.ReadLine();
                areEquals = string.Compare(email, confirm, StringComparison.InvariantCultureIgnoreCase) == 0;
            } while (!areEquals);
            return email;
        }
        public static string AskNewUsername() 
        {
            if (!MainConsole.IsSetup) return string.Empty;
            Console.Clear();
            Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
            Console.WriteLine("         Controle de usuários");
            Console.Write("Escolha Novo Apelido: ");
            string? username = Console.ReadLine();
            while ((username is null) || (username.Count() < 4)
                    || (!EmployeesMenuFunctions.employeeService.IsUsernameUnique(username))) {
                Console.Clear();
                Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
                Console.WriteLine("         Controle de usuários");
                Console.WriteLine("Nome de usuário não segue regras (pelo menos 4 caracteres ou usuário já existente).");
                Console.Write("Escolha Novo Apelido: ");
                username = Console.ReadLine();
            }
            return username;
        }

        public static string AskNewPassword() 
        {
            if (!MainConsole.IsSetup) return string.Empty;
            var passwordsMatch = false;
            var password = string.Empty;
            while (!passwordsMatch) {
                Console.Clear();
                password = ConsolePasswordReader.Read("Digite sua nova senha: ");
                passwordsMatch = string.Compare(ConsolePasswordReader.Read("Confirme sua nova senha: "), password) == 0;
                Console.WriteLine($"A verificação foi {(passwordsMatch ? "positiva" : "negativa")}");
                ConsoleUtils.AwaitForKey();
            }
            return password;
        }
        public static string AskNewName(string tipo) 
        {
            if (!MainConsole.IsSetup) return string.Empty;
            Console.Clear();
            Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
            Console.WriteLine($"         Controle de {tipo}");
            Console.Write("Nome Completo: ");
            string? name = Console.ReadLine();
            while ((name is null) || (name.Count() < 4)) {
                Console.Clear();
                Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
                Console.WriteLine($"         Controle de {tipo}");
                Console.WriteLine("Por favor, entre com o nome completo do usuário.");
                Console.Write("Nome Completo: ");
                name = Console.ReadLine();
            }
            return name;
        }
        public static string AskAddress() 
        {
            if (!MainConsole.IsSetup) return string.Empty;
            Console.Clear();
            Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
            Console.WriteLine("         Controle de clientes");
            Console.Write("Endereço: ");
            string? address = Console.ReadLine();
            while ((address is null) || (address.Count() < 4)) {
                Console.Clear();
                Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
                Console.WriteLine("         Controle de clientes");
                Console.WriteLine("Por favor, entre com o endereço completo.");
                Console.Write("Endereço: ");
                address = Console.ReadLine();
            }
            return address;
        }

        public static long AskDocument() {
            if (!MainConsole.IsSetup) return -1;
            Console.Clear();
            Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
            Console.WriteLine("         Controle de clientes");
            Console.Write("CPF: ");
            string? doc = Console.ReadLine();
            long documento;
            while ((doc is null) || (doc.Count() != 11) || (!long.TryParse(doc, out documento))) {
                Console.Clear();
                Console.WriteLine($"Bem vindo ao sistema AdaCredit.");
                Console.WriteLine("         Controle de clientes");
                Console.WriteLine("Por favor, entre com CPF correto.");
                Console.Write("CPF: ");
                doc = Console.ReadLine();
            }
            return documento;
        }

    }
}
