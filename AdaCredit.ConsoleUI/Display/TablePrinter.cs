using BetterConsoles.Tables;
using BetterConsoles.Tables.Models;
using BetterConsoles.Tables.Configuration;
using System.Drawing;
using BetterConsoles.Tables.Builders;
using BetterConsoles.Colors.Extensions;
using AdaCredit.Logical.Entities;

namespace AdaCredit.ConsoleUI.Display {
    internal static class TablePrinter 
    {

        static Color positiveMoney = Color.SeaGreen;
        static Color negativeMoney = Color.IndianRed;
        static string FormatMoney(decimal money) 
        {
            string valueStr = string.Format("{0:$#.00}", money);
            if (money >= 0) valueStr = valueStr.ForegroundColor(positiveMoney);
            else valueStr = valueStr.ForegroundColor(negativeMoney);
            return valueStr;
        }
        static string FormatDirection(int direction) 
        {
            if (direction == 0) return "Débito".ForegroundColor(Color.Red);
            else return "Crédito".ForegroundColor(Color.Blue);
        }

        static string FormatFailure(FailedTransactionCSVLine.FailureReason reason) 
        {
            if (reason == FailedTransactionCSVLine.FailureReason.IncompatibleType)
                return "Operação incompatível";
            else if (reason == FailedTransactionCSVLine.FailureReason.InsuficientFunds)
                return "Saldo insuficiente";
            else if (reason == FailedTransactionCSVLine.FailureReason.InvalidAccount)
                return "Conta inválida";
            else if (reason == FailedTransactionCSVLine.FailureReason.WrongDirection)
                return "Sentido errado";
            else if (reason == FailedTransactionCSVLine.FailureReason.NotPertinent)
                return "Não há ligação direta com a Ada Credit";
            else
                return "Conta inexistente";
        }

        static CellFormat headerFormat = new CellFormat() 
        {
            Alignment = Alignment.Center,
            ForegroundColor = Color.Magenta
        };

        static CellFormat dateFormat = new CellFormat(foregroundColor: Color.FromArgb(128, 129, 126));

        public static void PrintClients(List<Client> clients) 
        {
            Console.Clear();
            Table table = new TableBuilder(headerFormat)
                .AddColumn("Nome")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("CPF")
                .AddColumn("Email")
                .AddColumn("Endereço")
                .AddColumn("Data da abertura", rowsFormat: dateFormat)
                .AddColumn("Número da conta")
                .AddColumn("Saldo")
                    .RowFormatter<decimal>((x) => FormatMoney(x))
                    .RowsFormat()
                        .Alignment(Alignment.Right)
                .Build();
            table.Config = TableConfig.Unicode();
            foreach (var cli in clients) 
            {
                table.AddRow(cli.Name, cli.Document, cli.Email, cli.Address,
                             cli.CreatedAt, cli.Account.Number, cli.Account.Balance);
            }
            Console.WriteLine(table.ToString());
            ConsoleUtils.AwaitForKey();
        }
        public static void PrintInactiveClients(List<Client> clients) 
        {
            Console.Clear();
            Table table = new TableBuilder(headerFormat)
                .AddColumn("Nome")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("CPF")
                .AddColumn("Email")
                .AddColumn("Endereço")
                .AddColumn("Data de abertura", rowsFormat: dateFormat)
                .Build();
            table.Config = TableConfig.Unicode();
            foreach (var cli in clients) 
            {
                table.AddRow(cli.Name, cli.Document, cli.Email, cli.Address, cli.CreatedAt);
            }
            Console.WriteLine(table.ToString());
            ConsoleUtils.AwaitForKey();
        }

        public static void PrintEmployees(List<Employee> employees) 
        {
            Console.Clear();
            Table table = new TableBuilder(headerFormat)
                .AddColumn("Nome")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("Nome de usuário")
                .AddColumn("Email")
                .AddColumn("Último Login", rowsFormat: dateFormat)
                .AddColumn("Data de cadastro", rowsFormat: dateFormat)
                .Build();
            table.Config = TableConfig.Unicode();
            foreach (var emp in employees) 
            {
                table.AddRow(emp.Name, emp.Username, emp.Email, emp.LastLogin, emp.CreatedAt);
            }
            Console.WriteLine(table.ToString());
            ConsoleUtils.AwaitForKey();
        }

        public static void PrintTransactions(List<TransactionCSVLine> transactions) 
        {
            Console.Clear();
            Table table = new TableBuilder(headerFormat)
                .AddColumn("Banco de origem")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("Agência de origem")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("Conta de origem")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("Banco de destino")
                .AddColumn("Agência de destino")
                .AddColumn("Conta de destino")
                .AddColumn("Tipo de transação")
                    .RowsFormat()
                        .ForegroundColor(Color.Yellow)
                .AddColumn("Direção", rowsFormat: dateFormat)
                    .RowFormatter<int>((x) => FormatDirection(x))
                    .RowsFormat()
                        .Alignment(Alignment.Right)
                .AddColumn("Valor")
                    .RowFormatter<decimal>((x) => FormatMoney(x))
                    .RowsFormat()
                        .Alignment(Alignment.Right)
                .Build();
            table.Config = TableConfig.Unicode();
            foreach (var tra in transactions) 
            {
                table.AddRow(tra.originBankCode, tra.originBranchCode, tra.originAccount, tra.destBankCode,
                               tra.destBranchCode, tra.destAccount, tra.transactionType, tra.direction,
                               tra.value);
            }
            Console.WriteLine(table.ToString());
            ConsoleUtils.AwaitForKey();
        }

        public static void PrintFailedTransactions(List<FailedTransactionCSVLine> transactions) 
        {
            Console.Clear();
            Table table = new TableBuilder(headerFormat)
                .AddColumn("Banco de origem")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("Agência de origem")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("Conta de origem")
                    .RowsFormat()
                        .ForegroundColor(Color.FromArgb(100, 160, 179))
                .AddColumn("Banco de destino")
                .AddColumn("Agência de destino")
                .AddColumn("Conta de destino")
                .AddColumn("Tipo de transação")
                    .RowsFormat()
                        .ForegroundColor(Color.Yellow)
                .AddColumn("Direção", rowsFormat: dateFormat)
                    .RowFormatter<int>((x) => FormatDirection(x))
                    .RowsFormat()
                        .Alignment(Alignment.Right)
                .AddColumn("Valor")
                    .RowFormatter<decimal>((x) => FormatMoney(x))
                    .RowsFormat()
                        .Alignment(Alignment.Right)
                .AddColumn("Motivo de falha")
                    .RowFormatter<FailedTransactionCSVLine.FailureReason>((x) => FormatFailure(x))
                    .RowsFormat()
                        .Alignment(Alignment.Right)
                .Build();
            table.Config = TableConfig.Unicode();
            foreach (var tra in transactions) 
            {
                table.AddRow(tra.originBankCode, tra.originBranchCode, tra.originAccount, tra.destBankCode,
                               tra.destBranchCode, tra.destAccount, tra.transactionType, tra.direction,
                               tra.value, tra.failureReason);
            }
            Console.WriteLine(table.ToString());
            ConsoleUtils.AwaitForKey();
        }
    }
}
