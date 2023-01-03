using AdaCredit.Logical.Entities;
using AdaCredit.Logical.Repositories;
using Bogus;

namespace AdaCredit.Logical.Bogus {
    public static class BogusClient {
        public static void AutoBuildUniqueClient() {
            var faker = new Faker("pt_BR");
            JSONPersistence persistence = new(FileHandler.PathDataSave, JSONPersistence.PeristanceType.Client);
            ClientRepository repo = new(persistence);
            repo.LoadClienList();
            uint maxClients = faker.Random.UInt(1, 6);
            for (int i = 0; i < maxClients; i++) {
                var date = faker.Date.RecentDateOnly();
                DateTime dateTime = date.ToDateTime(new TimeOnly());
                long doc;
                do {
                    doc = faker.Random.Long(10000000000, 99999999999);
                } while (repo.GetClientByDocument(doc) != null);
                string acc;
                do {
                    acc = faker.Random.Replace("#####-#");
                } while (repo.GetClientByAccountNumber(acc) != null);
                Account account = new Account { BankCode = "777", BranchCode = "0001", Balance = faker.Finance.Amount(0, 999m, 2), Number = acc };
                var genEmployee = new Faker<Client>("pt_BR")
                    .StrictMode(true)
                    .RuleFor(o => o.Name, f => f.Name.FullName())
                    .RuleFor(o => o.Document, f => f.Random.Long(10000000000, 99999999999))
                    .RuleFor(o => o.Email, f => f.Internet.Email())
                    .RuleFor(o => o.Address, f => $"{f.Person.Address.State}, {f.Person.Address.City}, {f.Person.Address.Street}")
                    .RuleFor(o => o.CreatedAt, f => dateTime)
                    .RuleFor(o => o.Active, f => true)
                    .RuleFor(o => o.Account, f => account);
                repo.AddClient(genEmployee.Generate());
            }
            repo.SaveClientList();
        }
    }
}
