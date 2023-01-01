namespace AdaCredit.Logical.Entities {
    public static class FeeCalculator {
        public const decimal TEDFEE = 5.00m;
        public const decimal DOCFIXEDFEE = 1.00m;
        public const decimal TEFFEE = 0m;
        public static DateOnly FEEFREELIMIT = new(2022,11,30);
        public static decimal CalculateDocFee(decimal docValue) 
        {
            var fee = docValue * 0.01m;
            return fee <= 5m ? fee + 1 : 6m;
        }

        public static decimal CalculateFee(DateOnly dateOnly, decimal value, TransactionCSVLine.TransactionType type) {
            if (FEEFREELIMIT >= dateOnly) return 0;
            return type switch {
                TransactionCSVLine.TransactionType.TED => TEDFEE,
                TransactionCSVLine.TransactionType.DOC => CalculateDocFee(value),
                _ => 0
            };
        }
    }
}
