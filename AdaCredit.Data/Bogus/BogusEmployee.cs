using AdaCredit.Logical.Repositories;
using AdaCredit.Logical.Entities;
using Bogus;

namespace AdaCredit.Logical.Bogus {
    public static class BogusEmployee {
        public static void GenerateEmployeeRepository() {
            var empl = AutoBuildEmployeeRepository();
            empl.SaveEmployeeList();
        }
        internal static EmployeeRepository AutoBuildEmployeeRepository() {
            var faker = new Faker("pt_BR");
            JSONPersistence persistence = new(FileHandler.PathDataSave, JSONPersistence.PeristanceType.Employee);
            var employeeRepository = new EmployeeRepository(persistence);
            employeeRepository.LoadEmployeeList();
            var maxEmp = faker.Random.UInt(10, 40);
            for (int i = 0; i < maxEmp; i++) {
                employeeRepository.AddEmployee(AutoBuildEmployee());
            }
            return employeeRepository;
        }
        public static Employee AutoBuildEmployee() {
            var genEmployee = new Faker<Employee>("pt_BR")
                .StrictMode(true)
                .RuleFor(o => o.Username, f => f.Internet.UserName())
                .RuleFor(o => o.Name, f => f.Name.FullName())
                .RuleFor(o => o.Email, f => f.Internet.Email())
                .RuleFor(o => o.LastLogin, f => DateTime.Now)
                .RuleFor(o => o.HashedPassword, f => f.Random.String(20, '0', 'Z'))
                .RuleFor(o => o.PasswordSalt, f => f.Random.String(3, '0', 'Z'))
                .RuleFor(o => o.CreatedAt, f => DateTime.Now)
                .RuleFor(o => o.Active, f => true);
            return genEmployee.Generate();
        }
    }
}
