using AdaCredit.Logical.Entities;

namespace AdaCredit.Logical.Repositories {
    internal sealed class EmployeeRepository {
        private IDataPersistence dataPersistence;
        private List<Employee> employees = new List<Employee>();
        public IEnumerable<Employee> Employees{get => employees;}
        internal EmployeeRepository(IDataPersistence persistence) {
            dataPersistence = persistence;
            LoadEmployeeList();
        }
        public void LoadEmployeeList() {
            dataPersistence.LoadData(out employees);
        }
        public void SaveEmployeeList() {
            dataPersistence.SaveData(employees);
        }

        public void AddEmployee(Employee employee) {
            employees.Add(employee);
            SaveEmployeeList();
        }

        public Employee? GetEmployeeByUsername(string username) =>
            employees.FirstOrDefault(e => e.Username == username);

    }
}
