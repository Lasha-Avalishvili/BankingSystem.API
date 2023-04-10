namespace BankingSystem.Features.Reports
{
    public class TransactionsCountResponse
    {
        public TransactionTypesCount TransactionsInLast30Days { get; set; }
        public TransactionTypesCount TransactionsInLast6Months { get; set; }
        public TransactionTypesCount TransactionsInOneYear { get; set; }
    }
}
