using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using AdaCredit.Logical.Entities;


namespace AdaCredit.Logical.Repositories
{

    public sealed class TransactionFile
    {
        public string Name { get; set; }
        public string PathToFile { get; set; }

        public List<TransactionCSVLine> transactions { get; set; }

        public TransactionFile()
        {
            transactions = new List<TransactionCSVLine>();
            Name = string.Empty;
            PathToFile = string.Empty;
        }

        public TransactionFile(string path, string name)
        {
            transactions = new List<TransactionCSVLine>();
            Name = name;
            PathToFile = path;
        }

        public void ToCSV()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var writer = new StreamWriter(Path.Combine(PathToFile, Name)))
            using (var csv = new CsvWriter(writer, config)) {
                csv.WriteRecords(transactions);
            }
        }

        public void FromCSV()
        {
            if (!FileHandler.FileExists(Path.Combine(PathToFile, Name))) return;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var reader = new StreamReader(Path.Combine(PathToFile, Name)))
            using (var csv = new CsvReader(reader, config))
            {
                transactions = csv.GetRecords<TransactionCSVLine>().ToList();
            }
        }

    }
}
