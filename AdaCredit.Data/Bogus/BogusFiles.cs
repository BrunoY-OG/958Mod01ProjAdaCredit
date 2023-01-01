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
            TransactionFile file = AutoBuildCSV(acc);
            file.ToCSV();
        }
        public static TransactionFile AutoBuildCSV(string[] accounts, bool Ada = false) {
            var faker = new Faker("en");
            var file = new TransactionFile();
            file.PathToFile = FileHandler.PathToPending;
            do {
                if (Ada) {
                    file.Name = ADABANK;
                }
                else
                    file.Name = faker.PickRandom(BANKNAMES);
                DateOnly date = faker.Date.BetweenDateOnly(new DateOnly(2022, 11, 28),
                                                            new DateOnly(2022, 12, 03));
                file.Name += "-" + date.ToString("yyyyMMdd") + ".csv";
            } while (FileHandler.FileExists(Path.Combine(FileHandler.PathToPending, file.Name)));
            int maxLines = (int)faker.Random.UInt(20, 30);

            file.transactions.Add(AutoBuildLine(accounts, Ada, -1));
            for (int i = 0; i < maxLines; i++) {
                var dir = faker.Random.Int(0, 1);
                if (dir == -1)
                    file.transactions.Add(AutoBuildLine());
                else
                    file.transactions.Add(AutoBuildLine(accounts, Ada, dir));
            }
            file.transactions.Add(AutoBuildLine(accounts, Ada, -1));

            return file;
        }

        public static TransactionCSVLine AutoBuildLine(string[] accounts, bool Ada = false, int dir = 0) {
            var genTransactionLine = new Faker<TransactionCSVLine>()
                .StrictMode(true)
                .RuleFor(o => o.originBankCode, f => {
                    if (dir == 0 || Ada)
                        return "777";
                    else
                        return f.Random.Replace("###");
                })
                .RuleFor(o => o.originBranchCode, f => {
                    if (dir == 0 || Ada)
                        return "0001";
                    else
                        return f.Random.Replace("####");
                })
                .RuleFor(o => o.originAccount, f => {
                    if (dir == 0 || Ada)
                        return f.PickRandom(accounts).ToggleAccountDash();
                    else
                        return f.Random.Replace("######");
                })
                .RuleFor(o => o.destBankCode, f => {
                    if (dir == 1 || Ada)
                        return "777";
                    else
                        return f.Random.Replace("###");
                })
                .RuleFor(o => o.destBranchCode, f => {
                    if (dir == 1 || Ada)
                        return "0001";
                    else
                        return f.Random.Replace("####");
                })
                .RuleFor(o => o.destAccount, f => {
                    if (dir == 1 || Ada)
                        return f.PickRandom(accounts).ToggleAccountDash();
                    else
                        return f.Random.Replace("######");
                })
                .RuleFor(o => o.transactionType, f => {
                    if (Ada)
                        return TransactionCSVLine.TransactionType.TEF;
                    if (dir == 1)
                        return f.PickRandom<TransactionCSVLine.TransactionType>();
                    else
                        return f.PickRandomWithout(TransactionCSVLine.TransactionType.TEF);
                })
                .RuleFor(o => o.direction, f => {
                    return dir;
                })
                .RuleFor(o => o.value, f => f.Finance.Amount(-999m, 999m, 2));
            return genTransactionLine.Generate();
        }

        public static TransactionCSVLine AutoBuildLine() {
            var genTransactionLine = new Faker<TransactionCSVLine>()
                .StrictMode(true)
                .RuleFor(o => o.originBankCode, f => f.Random.Replace("###"))
                .RuleFor(o => o.originBranchCode, f => f.Random.Replace("####"))
                .RuleFor(o => o.originAccount, f => f.Random.Replace("######"))
                .RuleFor(o => o.destBankCode, f => f.Random.Replace("###"))
                .RuleFor(o => o.destBranchCode, f => f.Random.Replace("####"))
                .RuleFor(o => o.destAccount, f => f.Random.Replace("######"))
                .RuleFor(o => o.transactionType, f => f.PickRandomWithout<TransactionCSVLine.TransactionType>())
                .RuleFor(o => o.direction, f => f.Random.Int(0, 1))
                .RuleFor(o => o.value, f => f.Finance.Amount(-999m, 999m, 2));
            return genTransactionLine.Generate();
        }

    }
}
