using AdaCredit.Logical.Repositories;
using AdaCredit.Logical.Entities;
using static BCrypt.Net.BCrypt;
using System.Windows.Markup;

namespace AdaCredit.Logical.Services
{
    public sealed class EmployeeService 
    {
        private static readonly string PATH = Path.Combine(FileHandler.PathDataSave, "Employees.json");
        private EmployeeRepository Repository { get; init; }
        public List<Employee> SafeEmployeesList 
        {
            get 
            {
                Repository.LoadEmployeeList();
                return Repository.Employees.Select(SafeEmployee).ToList();
            }
        }
        public bool IsFirstLogin 
        {
            get 
            {
                Repository.LoadEmployeeList();
                return Repository.Employees.Count() == 0;
            }
        }


        public EmployeeService(IDataPersistence dataPersistence) 
        {
            Repository = new EmployeeRepository(dataPersistence);
        }

        private Employee SafeEmployee(Employee employee) =>
            new Employee {
                Username = employee.Username,
                Name = employee.Name,
                Email = employee.Email,
                LastLogin = employee.LastLogin,
                HashedPassword = "",
                PasswordSalt = "",
                CreatedAt = employee.CreatedAt,
                Active = employee.Active
            };


        public bool IsUsernameUnique(string userName) 
        {
            Repository.LoadEmployeeList();
            return Repository.GetEmployeeByUsername(userName) == null;
        }

        private Employee? CheckPassword(string userName, string password) 
        {
            Repository.LoadEmployeeList();
            var employee = Repository.GetEmployeeByUsername(userName);
            if ((employee != null) && (Verify(password, employee.HashedPassword)))  return employee;
            return null;
        }

        public bool Login(string userName, string password)
        {
            Repository.LoadEmployeeList();
            if (IsFirstLogin) 
            {
                if ((userName == "user") && (password == "pass")) return true;
                return false;
            }
            var employee = CheckPassword(userName, password);
            if ((employee != null) && (employee.Active)) 
            {
                employee.LastLogin = DateTime.Now;
                Repository.SaveEmployeeList();
                return true;
            }
            return false;
        }

        public bool ChangeUsername(string oldUsername, string newUsername) 
        {
            Repository.LoadEmployeeList();
            Employee? employee = Repository.GetEmployeeByUsername(oldUsername);
            if (employee != null) 
            {
                employee.Username = newUsername;
                Repository.SaveEmployeeList();
                return true;
            }
            return false;
        }

        public bool ChangeName(string userName, string name) 
        {
            Repository.LoadEmployeeList();
            Employee? employee = Repository.GetEmployeeByUsername(userName);
            if (employee != null) {
                employee.Name = name;
                Repository.SaveEmployeeList();
                return true;
            }
            return false;
        }


        public bool ChangeEmail(string userName, string email) {
            Repository.LoadEmployeeList();
            Employee? employee = Repository.GetEmployeeByUsername(userName);
            if (employee != null) 
            {
                employee.Email = email;
                Repository.SaveEmployeeList();
                return true;
            }
            return false;
        }

        public bool ChangePassword(string userName, string newPassword) {
            Repository.LoadEmployeeList();
            var employee = Repository.GetEmployeeByUsername(userName);
            if (employee != null) 
            {
                employee.PasswordSalt = GenerateSalt();
                employee.HashedPassword = HashPassword(newPassword, employee.PasswordSalt);
                Repository.SaveEmployeeList();
                return true;
            }
            return false;
        }

        public bool SetEmployeeActive(string username, bool set) {
            Repository.LoadEmployeeList();
            Employee? employee = Repository.GetEmployeeByUsername(username);
            if (employee != null) 
            {
                employee.Active = set;
                Repository.SaveEmployeeList();
                return true;
            }
            return false;
        }

        public List<Employee> EmployeeReport() =>
            SafeEmployeesList.Where(e => e.Active).ToList();

        public bool AddNewEmployee(string userName, string password) {
            if (!IsUsernameUnique(userName)) return false;
            var salt = GenerateSalt();
            Repository.AddEmployee(new() 
                                        {
                                            Username = userName, 
                                            PasswordSalt = salt,
                                            HashedPassword = HashPassword(password, salt),
                                            Active = true
                                        });
            return true;
        }
    }
}
