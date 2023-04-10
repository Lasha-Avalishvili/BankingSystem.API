using BankingSystem.DB.Entities;

namespace BankingSystem.Features.Reports
{
    public interface IReportsService
    {
        public Task<UsersCountResponse> GetUsersRegistered(DateTime firstDayOfYear, DateTime lastYearSameDay, DateTime last30Days);
        public Task<TransactionsCountResponse> GetTransactionsCount(DateTime last6Months, DateTime lastYearSameDay, DateTime last30Days); // new
        public Task<CalculateIncomeResponse> CalculateIncome(DateTime last30Days, DateTime last6Months, DateTime lastYearSameDay);
        public Task<AverageTransactionFeeResponse> CalculateAverageTransactionFee(DateTime date);
        public Task<Dictionary<DateTime, int>> GetTrasnactionsChart(DateTime date);
        public Task<TotalCashoutsResponse> GetTotalCashout();
    }

    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public async Task<UsersCountResponse> GetUsersRegistered(DateTime firstDayOfYear, DateTime lastYearSameDay, DateTime last30Days)
        {
            var usersThisYear = await _reportsRepository.GetUsersCountAsync(firstDayOfYear);
            var usersIn1Year = await _reportsRepository.GetUsersCountAsync(lastYearSameDay);
            var usersIn30Days = await _reportsRepository.GetUsersCountAsync(last30Days);

            return new UsersCountResponse { UsersThisYear = usersThisYear, UsersInOneYear = usersIn1Year, UsersInLast30Days = usersIn30Days };
        }

        public async Task<TransactionsCountResponse> GetTransactionsCount(DateTime last30Days, DateTime last6Months, DateTime lastYearSameDay)
        {
            var transactionsIn30Days = await CountTransactionsByDate(last30Days);
            var transactionsIn6Months = await CountTransactionsByDate(last6Months);
            var transactionsInOneYear = await CountTransactionsByDate(lastYearSameDay);

            return new TransactionsCountResponse { TransactionsInLast30Days = transactionsIn30Days, TransactionsInLast6Months = transactionsIn6Months, TransactionsInOneYear = transactionsInOneYear };
        }

        public async Task<TransactionTypesCount> CountTransactionsByDate(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var groupedTransactions = transactions.GroupBy(t => t.TransactionType)
                .Select(g => new
                {
                    TransactionType = g.Key,
                    Count = g.Count()
                });

            var all = groupedTransactions.Sum(t => t.Count);
            var inner = groupedTransactions.FirstOrDefault(t => t.TransactionType == TransactionType.Inner)?.Count ?? 0;
            var outer = groupedTransactions.FirstOrDefault(t => t.TransactionType == TransactionType.Outer)?.Count ?? 0;
            var ATM = groupedTransactions.FirstOrDefault(t => t.TransactionType == TransactionType.ATM)?.Count ?? 0;

            return new TransactionTypesCount { ATM = ATM, inner = inner, outer = outer, total = all };
        }

        public async Task<CalculateIncomeResponse> CalculateIncome(DateTime last30Days, DateTime last6Months, DateTime lastYearSameDay)
        {
            var incomeIn30Days = await CalculateFees(last30Days);
            var incomeIn6Months = await CalculateFees(last6Months);
            var incomeIn1Year = await CalculateFees(lastYearSameDay);

            return new CalculateIncomeResponse { IncomeInLast30Days = incomeIn30Days, IncomeInLast6Months = incomeIn6Months, IncomeIn1Year = incomeIn1Year };
        }

        public async Task<FeeCurrencies> CalculateFees(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var transactionFees = transactions.Select(t => new
            {
                t.FeeInUSD,
                t.FeeInGEL,
                t.FeeInEUR
            })
                .ToList();

            var allFeeInUSD = transactionFees.Sum(t => t.FeeInUSD);
            var allFeeInGEL = transactionFees.Sum(t => t.FeeInGEL);
            var allFeeInEUR = transactionFees.Sum(t => t.FeeInEUR);

            return new FeeCurrencies
            {
                GEL = allFeeInGEL,
                USD = allFeeInUSD,
                EUR = allFeeInEUR,
            };
        }

        public async Task<AverageTransactionFeeResponse> CalculateAverageTransactionFee(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var transactionFees = transactions.Select(t => new
            {
                t.FeeInUSD,
                t.FeeInGEL,
                t.FeeInEUR
            })
                .ToList();

            var allFeeInUSD = transactionFees.Sum(t => t.FeeInUSD);
            var allFeeInGEL = transactionFees.Sum(t => t.FeeInGEL);
            var allFeeInEUR = transactionFees.Sum(t => t.FeeInEUR);

            var totalCount = transactionFees.Count();
            var avgFeeInGEL = totalCount > 0 ? allFeeInGEL / totalCount : 0;
            var avgFeeInUSD = totalCount > 0 ? allFeeInUSD / totalCount : 0;
            var avgFeeInEUR = totalCount > 0 ? allFeeInEUR / totalCount : 0;

            var avgFees = new FeeCurrencies
            {
                GEL = Math.Round(avgFeeInGEL, 2),
                USD = Math.Round(avgFeeInUSD, 2),
                EUR = Math.Round(avgFeeInEUR, 2)
            };
            return new AverageTransactionFeeResponse { AverageTransactionFee = avgFees };
        }

        public async Task<Dictionary<DateTime, int>> GetTrasnactionsChart(DateTime date)
        {
            var transactions = await _reportsRepository.GetTransactionsAsync(date);
            var transactionCountByDay = new Dictionary<DateTime, int>();

            for (var startdate = date; date <= DateTime.UtcNow; date = date.AddDays(1))
            {
                var transactionsCount = transactions.Count(x => x.CreatedAt.Date == date.Date);
                transactionCountByDay.Add(date, transactionsCount);
            }

            return transactionCountByDay;
        }

        public async Task<TotalCashoutsResponse> GetTotalCashout()
        {
            var result = new TotalCashoutsResponse();
            result.TotalCashoutsInGel = await _reportsRepository.GettotalCashoutInGELAsync();
            return result;
        }
    }

    public class FeeCurrencies
    {
        public decimal GEL { get; set; }
        public decimal USD { get; set; }
        public decimal EUR { get; set; }
    }

    public class TransactionTypesCount
    {
        public long inner { get; set; }
        public long outer { get; set; }
        public long ATM { get; set; }
        public long total { get; set; }
    }

}

