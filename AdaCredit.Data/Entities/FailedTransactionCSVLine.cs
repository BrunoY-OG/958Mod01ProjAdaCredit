namespace AdaCredit.Logical.Entities {
    public sealed class FailedTransactionCSVLine {

        public enum FailureReason {
            InsuficientFunds,
            InvalidAccount,
            InexistentAccount,
            IncompatibleType,
            WrongDirection,
            NotApplicable
        }

        public string originBankCode { get; set; }
        public string originBranchCode { get; set; }
        public string originAccount { get; set; }

        public string destBankCode { get; set; }
        public string destBranchCode { get; set; }
        public string destAccount { get; set; }

        public TransactionCSVLine.TransactionType transactionType { get; set; }

        public int direction { get; set; }

        public decimal value { get; set; }

        public FailureReason failureReason { get; set; }

        public FailedTransactionCSVLine(TransactionCSVLine line, FailureReason reason) {
            originBankCode = line.originBankCode;
            originBranchCode = line.originBranchCode;
            originAccount = line.originAccount;
            destBankCode = line.destBankCode;
            destBranchCode = line.destBranchCode;
            destAccount = line.destAccount;
            transactionType = line.transactionType;
            direction = line.direction;
            value = line.value;
            failureReason = reason;
        }

        public FailedTransactionCSVLine(FailedTransactionCSVLine line) {
            originBankCode = line.originBankCode;
            originBranchCode = line.originBranchCode;
            originAccount = line.originAccount;
            destBankCode = line.destBankCode;
            destBranchCode = line.destBranchCode;
            destAccount = line.destAccount;
            transactionType = line.transactionType;
            direction = line.direction;
            value = line.value;
            failureReason = line.failureReason;
        }
    }
}
