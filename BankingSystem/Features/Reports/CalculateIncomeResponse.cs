namespace BankingSystem.Features.Reports
{
    public class CalculateIncomeResponse
    {
        public FeeCurrencies IncomeInLast30Days { get; set; }
        public FeeCurrencies IncomeInLast6Months { get; set; }
        public FeeCurrencies IncomeIn1Year { get; set; }
    }
}
