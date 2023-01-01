using System;
using System.IO;
using System.Linq.Expressions;

namespace AdaCredit.Logical.Entities
{
    public static class FileHandler {
        public static readonly string PathToTransactions = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            "Transactions");
        public static readonly string PathToPending = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            "Transactions" ,"Pending");
        public static readonly string PathToFailed = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            "Transactions", "Failed");
        public static readonly string PathToCompleted = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            "Transactions", "Completed");
        public static readonly string PathDataSave = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

        public static void Setup() {
            if (!Directory.Exists(PathToTransactions))
                Directory.CreateDirectory(PathToTransactions);
            if (!Directory.Exists(PathToPending))
                Directory.CreateDirectory(PathToPending);
            if (!Directory.Exists(PathToFailed))
                Directory.CreateDirectory(PathToFailed);
            if (!Directory.Exists(PathToCompleted))
                Directory.CreateDirectory(PathToCompleted);
            if (!Directory.Exists(PathDataSave))
                Directory.CreateDirectory(PathDataSave);
        }

        internal static string? Load(string path) {
            try {
                return File.ReadAllText(path);
            }
            catch {
                return null;
            }
        }


        public static bool FileExists(string path) {
            try {
                return File.Exists(path);
            }
            catch {
                return false;
            }
        }

        internal static bool CreateEmptyIfDoesntExist(string path) {
            try {
                if (!File.Exists(path)) {
                    File.Create(path).Close();
                }
                return true;
            }
             catch{
                return false;
            }
        }

        public static string[] GetFileNames(string path) {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Directory.GetFiles(path);
        }
    }
}
