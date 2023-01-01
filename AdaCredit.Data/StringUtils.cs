using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace AdaCredit.Logical {
    public static class StringUtils {
        public static bool IsValidEmail(this string email) {
            Regex rx = new Regex(@"^[\w\.\-]+\@[\w]+\.[\w]+$",
              RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(email);
            if (matches.Count() > 0) return true;
            return false;
        }
        private static bool isCSV(this string csv) =>
            csv.ToLower().EndsWith(".csv");

        private static bool isCompleted(this string csv) =>
            csv.ToLower().EndsWith("-completed") || csv.ToLower().EndsWith("-completed.csv");
        private static bool isFailed(this string csv) =>
            csv.ToLower().EndsWith("-failed") || csv.ToLower().EndsWith("-failed.csv");

        private static string CSVRemoveCompleted(this string csvName) {
            if (csvName.isCompleted())
                return csvName.Substring(0, csvName.IndexOf("-completed"));
            else return csvName;
        }
        private static string CSVRemoveFailed(this string csvName) {
            if (csvName.isFailed())
                return csvName.Substring(0, csvName.IndexOf("-failed"));
            else return csvName;
        }
        public static string CSVExtract(this string csvName) {
            var name = csvName.CSVRemoveCompleted();
            name = name.CSVRemoveFailed();
            if (name.isCSV())
              return name.Substring(0, csvName.Length - 4);
            else return name;
        }

        public static string CSVExtractDate(this string csvName) {
            var extract = csvName.CSVExtract();
            return extract.Substring(extract.Length - 8);
        }

        public static string CSVExtractBank(this string csvName) {
            var extract = csvName.CSVExtract();
            return extract.Substring(0, extract.Length - 9);
        }

        public static string ToggleAccountDash(this string account) {
            if (account.IndexOf('-') == -1)
                return account.Insert(5, "-");
            else 
                return account.Remove(account.IndexOf('-'), 1);
        }
    }
}
