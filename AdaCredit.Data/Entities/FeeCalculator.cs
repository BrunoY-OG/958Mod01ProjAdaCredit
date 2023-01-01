namespace AdaCredit.Logical.Entities {
    public static class FeeCalculator {
        public const decimal TEDFEE = 5.00m;
        public const decimal DOCFIXEDFEE = 1.00m;
        public const decimal TEFFEE = 0m;
        public static decimal CalculateDocFee(decimal docValue) 
        {
            var fee = docValue * 0.01m;
            return fee <= 5m ? fee : 5m;
        }

        public static decimal CalculateFee(decimal value, TransactionCSVLine.TransactionType type) =>
            type switch {
                TransactionCSVLine.TransactionType.TED => value * TEDFEE,
                TransactionCSVLine.TransactionType.DOC => CalculateDocFee(value),
                _ => value
            };
    }
}
