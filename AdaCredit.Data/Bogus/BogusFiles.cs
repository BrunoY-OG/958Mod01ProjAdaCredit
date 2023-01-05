using AdaCredit.Logical.Entities;
using AdaCredit.Logical.Repositories;
using Bogus;


namespace AdaCredit.Logical.Bogus {

    public static class BogusFiles {
        private const string ADABANK = "AdaCredit";
        private static string[] BANKNAMES = { "BankC-3", "BankA-1", "BankB-2" };
        public static void GenerateCSV() {

            JSONPersistence clientPersistence = new JSONPersistence(FileHandler.PathDataSave, JSONPersistence.PeristanceType.Client);
            ClientRepository clientRepository = new(clientPersistence);
            string[] acc = clientRepository.Clients.Select(c => c.Account.Number).ToArray();
            for (int i = 0; i < 6; i++) {
                TransactionFile file = AutoBuildCSV(acc);
                file.ToCSV();
            }
        }
        public static TransactionFile AutoBuildCSV(string[] accounts) {
            var faker = new Faker("en");
            var file = new TransactionFile();
            file.PathToFile = FileHandler.PathToPending;
            do {
                file.Name = faker.PickRandom(BANKNAMES);
                DateOnly date = faker.Date.BetweenDateOnly(new DateOnly(2022, 11, 28),
                                                            new DateOnly(2022, 12, 03));
                file.Name += "-" + date.ToString("yyyyMMdd") + ".csv";
            } while (FileHandler.FileExists(Path.Combine(FileHandler.PathToPending, file.Name)));
            int maxLines = (int)faker.Random.UInt(20, 30);


            for (int i = 0; i < maxLines; i++) {
                var dir = faker.Random.Int(0, 4);
                file.transactions.Add(AutoBuildLine(accounts, dir));
            }

            return file;
        }

        public static TransactionCSVLine AutoBuildLine(string[] accounts, int dir = 0) {
            var genTransactionLine = new Faker<TransactionCSVLine>()
                .StrictMode(true)
                .RuleFor(o => o.originBankCode, f => {
                    if ((dir >= 0 && dir <= 1) || dir == 4)
                        return "777";
                    return f.Random.Replace("###");
                })
                .RuleFor(o => o.originBranchCode, f => {
                    if ((dir >= 0 && dir <= 1) || dir == 4)
                        return "0001";
                    return f.Random.Replace("####");
                })
                .RuleFor(o => o.originAccount, f => {
                    if ((dir >= 0 && dir <= 1) || dir == 4)
                        return f.PickRandom(accounts).ToggleAccountDash();
                    return f.Random.Replace("######");
                })
                .RuleFor(o => o.destBankCode, f => {
                    if (dir >= 2 && dir <= 4)
                        return "777";
                    return f.Random.Replace("###");
                })
                .RuleFor(o => o.destBranchCode, f => {
                    if (dir >= 2 && dir <= 4)
                        return "0001";
                    return f.Random.Replace("####");
                })
                .RuleFor(o => o.destAccount, f => {
                    if (dir >= 2 && dir <= 4)
                        return f.PickRandom(accounts).ToggleAccountDash();
                    return f.Random.Replace("######");
                })
                .RuleFor(o => o.transactionType, f => {
                    if ((dir == 0) || (dir == 3))
                        return f.PickRandomWithout(TransactionCSVLine.TransactionType.TEF);
                    return f.PickRandom<TransactionCSVLine.TransactionType>();
                })
                .RuleFor(o => o.value, f => f.Finance.Amount(0, 99m, 2));
            return genTransactionLine.Generate();
        }


    }
}
