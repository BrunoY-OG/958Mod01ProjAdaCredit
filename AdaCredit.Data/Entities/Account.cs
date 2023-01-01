using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdaCredit.Logical.Entities {
    public sealed class Account {
        //caso mais de uma conta por cliente, ou utilizando DB
        //public int ID { get; init; }
        //public int ClientID { get; init; }
        public string BankCode { get; set; }
        public string BranchCode { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public bool Active { get; set; }
        public Account() { }

    }
}
