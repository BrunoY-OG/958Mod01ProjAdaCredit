using System.Text.Json;

namespace AdaCredit.Logical.Entities {
    public sealed class JSONPersistence: IDataPersistence {
        private string _path;
        public JSONPersistence(string path, PeristanceType type) {
            if (type == PeristanceType.Employee) _path = Path.Combine(path, EMPLOYEEFILENAME);
            else if (type == PeristanceType.Client) _path = Path.Combine(path, CLIENTFILENAME);
        }
        public const string CLIENTFILENAME = "Clients.json";
        public const string EMPLOYEEFILENAME = "Employees.json";
        public enum PeristanceType {
            Client,
            Employee
        }

        public void LoadData<T>(out List<T> values) {
            FileHandler.CreateEmptyIfDoesntExist(_path);
            string? jsonString = FileHandler.Load(_path);
            if ((jsonString != default) && (jsonString != string.Empty) && (jsonString != Environment.NewLine))
                values = JsonSerializer.Deserialize<List<T>>(jsonString)!;
            else values = new List<T>();
        }

        public void SaveData<T>(List<T> values) {
            FileHandler.CreateEmptyIfDoesntExist(_path);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(values, options);
            File.WriteAllText(_path, jsonString);
        }
    }
}
