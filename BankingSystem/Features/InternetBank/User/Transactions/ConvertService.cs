using BankingSystem.DB.Entities;
using Newtonsoft.Json;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public interface IConvertService
    {
        public Task<decimal> ConvertCurrency(decimal amount, string SenderAccountCurrency, string recipientAccountCurrency);
        public Task<decimal> GetRate(string senderAccountCurrency, string recipientAccountCurrency);
        public Task<decimal> GetDailyLimitForEachCurrency(decimal amountOfLimit, string limitCurrency, string senderAccountCurrency);
    }
    public class ConvertService : IConvertService
    {
        
        private readonly Dictionary<string, decimal> _exchangeRates = new Dictionary<string, decimal>
        {
            { "GEL", 1},
            { "USD", 2.65m },
            { "EUR", 2.73m }
        };

        private readonly ITransactionRepository _transactionRepository;

        public ConvertService(ITransactionRepository transactionRepository)
        {
            _transactionRepository  = transactionRepository;
        }
       

        public async Task<decimal> ConvertCurrency(decimal amount, string senderAccountCurrency, string recipientAccountCurrency)
        {
            var currencyRates = await _transactionRepository.GetCurrenciesAsync();

            var sourceRate = currencyRates.FirstOrDefault(cr => cr.QuoteCurrency == recipientAccountCurrency)?.Rate ?? 1;
            var targetRate = currencyRates.FirstOrDefault(cr => cr.QuoteCurrency == senderAccountCurrency)?.Rate ?? 1 ;

            var convertedAmount = amount * (targetRate / sourceRate);

            return convertedAmount;
        }

        public async Task<decimal> GetRate(string senderAccountCurrency, string recipientAccountCurrency)
        {
            var currencyRates = await _transactionRepository.GetCurrenciesAsync();

            var sourceRate = currencyRates.FirstOrDefault(cr => cr.QuoteCurrency == recipientAccountCurrency)?.Rate ?? 1;
            var targetRate = currencyRates.FirstOrDefault(cr => cr.QuoteCurrency == senderAccountCurrency)?.Rate ?? 1;

            var rate = targetRate / sourceRate;
            return rate;
        }

        public async Task<decimal> GetDailyLimitForEachCurrency(decimal amountOfLimit, string limitCurrency, string senderAccountCurrency)
        {
            var currencyRates = await _transactionRepository.GetCurrenciesAsync();
            var sourceRate = currencyRates.FirstOrDefault(cr => cr.QuoteCurrency == limitCurrency)?.Rate ?? 1;
            var targetRate = currencyRates.FirstOrDefault(cr => cr.QuoteCurrency == senderAccountCurrency)?.Rate ?? 1;

            var convertedLimit = amountOfLimit * (targetRate / sourceRate);

            return convertedLimit;
        }
    }
}
