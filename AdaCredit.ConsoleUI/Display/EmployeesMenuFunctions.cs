using GetPass;
using AdaCredit.Logical.Services;
using System.Drawing;
using BetterConsoles.Colors.Extensions;
using AdaCredit.Logical.Entities;
using ConsoleTools;

namespace AdaCredit.ConsoleUI.Display
{
    public static class EmployeesMenuFunctions
    {
        public static EmployeeService employeeService;

        public static bool Welcome() 
        {
            bool login = false;
            while (!login) {
                login = Login();
            }
            if (employeeService.IsFirstLogin) {
                CreateNewUser();
            }
            return true;
        }
        public static bool Login()
        {
            if (!MainConsole.IsSetup) return false;
            Console.Clear();
            Console.WriteLine("Bem vindo ao sistema AdaCredit.");
            Console.WriteLine("         Login");
            Console.Write("Usuário: ");
            string? username = Console.ReadLine();
            while (username is null || username.Count() < 4)
            {
                Console.Clear();
                Console.WriteLine("Bem vindo ao sistema AdaCredit.");
                Console.WriteLine("         Login");
                Console.WriteLine("Nome de usuário não segue regras (pelo menos 4 caracteres).");
                Console.Write("Usuário: ");
                username = Console.ReadLine();
            }
            return employeeService.Login(username, ConsolePasswordReader.Read("Senha: "));
        }

        public static Employee? SelectEmployee() 
        {
            if (!MainConsole.IsSetup) return null;
            Employee result = null;
            var employeeSelector = Menu.SelectMenu("AdaCredit / Funcionários / Selecionar");
            List <Employee> employees = employeeService.SafeEmployeesList;
            foreach (Employee employee in employees) {
                employeeSelector.Add($"{employee.Username}: {employee.Name}", () => {
                    result = employee;
                    employeeSelector.CloseMenu();
                });
            }
            employeeSelector.Show();
            return result;
        }


        public static void ChangePassword() {
            if (!MainConsole.IsSetup) return;
            Employee? employee = SelectEmployee();
            if (employee is null) return;
            while (!employeeService.ChangePassword(employee.Username, InputReceiver.AskNewPassword())) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar senha, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }

        public static void ChangeUsername() {
            if (!MainConsole.IsSetup) return;
            Employee? employee = SelectEmployee();
            if (employee is null) return;
            while (!employeeService.ChangeUsername(employee.Username, InputReceiver.AskNewUsername())) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar apelido, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }

        public static void ChangeName() {
            if (!MainConsole.IsSetup) return;
            Employee? employee = SelectEmployee();
            if (employee is null) return;
            while (!employeeService.ChangeName(employee.Username, InputReceiver.AskNewName("usuários"))) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar nome, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }


        public static void ChangeEmail() {
            if (!MainConsole.IsSetup) return;
            Employee? employee = SelectEmployee();
            if (employee is null) return;
            while (!employeeService.ChangeName(employee.Username, InputReceiver.AskNewEmail("usuários"))) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar email, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }

        public static void Deactivate() {
            if (!MainConsole.IsSetup) return;
            Employee? employee = SelectEmployee();
            if (employee is null) return;
            employeeService.SetEmployeeActive(employee.Username, false);
        }

        public static void CreateNewUser() 
        {
            if (!MainConsole.IsSetup) return;
            var username = InputReceiver.AskNewUsername();
            while (!employeeService.AddNewEmployee(username, InputReceiver.AskNewPassword())) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar nome de usuário e senha, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
                username = InputReceiver.AskNewUsername();
            }
            while (!employeeService.ChangeName(username, InputReceiver.AskNewName("usuários"))) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar nome, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
            while (!employeeService.ChangeEmail(username, InputReceiver.AskNewEmail("usuários"))) {
                Console.WriteLine($"{Environment.NewLine}Não foi possível alterar email, tente novamente.".ForegroundColor(Color.Red));
                ConsoleUtils.AwaitForKey();
            }
        }

        public static void ReportActiveEmployees() =>
            TablePrinter.PrintEmployees(employeeService.EmployeeReport());
    }
}