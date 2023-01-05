namespace AdaCredit.Logical.Entities {
    public sealed class TransactionCSVLine {
        public enum TransactionType {
            DOC,
            TED,
            TEF
        }

        public string originBankCode { get; set; }
        public string originBranchCode { get; set; }
        public string originAccount { get; set; }

        public string destBankCode { get; set; }
        public string destBranchCode { get; set; }
        public string destAccount { get; set; }

        public TransactionType transactionType { get; set; }

        public decimal value { get; set; }

    }
}
