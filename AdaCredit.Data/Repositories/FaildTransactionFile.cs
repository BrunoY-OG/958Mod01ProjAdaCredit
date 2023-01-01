using AdaCredit.Logical.Entities;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace AdaCredit.Logical.Repositories {
    public sealed class FailedTransactionFile {
        public string Name { get; set; }
        public string PathToFile { get; set; }

        public List<FailedTransactionCSVLine> transactions { get; set; }

        public FailedTransactionFile() {
            transactions = new List<FailedTransactionCSVLine>();
        }

        public FailedTransactionFile(string path, string name) {
            transactions = new List<FailedTransactionCSVLine>();
            Name = name;
            PathToFile = path;
        }

        public void ToCSV() {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false,
            };
            using (var writer = new StreamWriter(Path.Combine(PathToFile, Name)))
            using (var csv = new CsvWriter(writer, config)) {
                csv.WriteRecords(transactions);
            }
        }

        public void FromCSV() {
            if (!FileHandler.FileExists(Path.Combine(PathToFile, Name))) return;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false,
            };
            using (var reader = new StreamReader(Path.Combine(PathToFile, Name)))
            using (var csv = new CsvReader(reader, config)) {
                transactions = csv.GetRecords<FailedTransactionCSVLine>().ToList();
            }
        }

        public static FailedTransactionFile NewFromCSV(string path, string name) {
            FailedTransactionFile failedTransactionFile = new();
            failedTransactionFile.PathToFile = path;
            failedTransactionFile.Name = name;
            failedTransactionFile.FromCSV();
            return failedTransactionFile;
        }

    }
}
