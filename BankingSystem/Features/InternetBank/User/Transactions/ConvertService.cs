using BankingSystem.DB.Entities;
using Newtonsoft.Json;

namespace BankingSystem.Features.InternetBank.User.Transactions
{
    public interface IConvertService
    {
        Task<decimal> ConvertCurrency(decimal amount, string SenderAccountCurrency, string recipientAccountCurrency);
        decimal GetRate(string SenderAccountCurrency, string recipientAccountCurrency);
   
        decimal GetDailyLimitForEachCurrency(decimal amountOfLimit, string limitCurrency, string senderAccountCurrency);
    }
    public class ConvertService : IConvertService
    {
        
        private readonly Dictionary<string, decimal> _exchangeRates = new Dictionary<string, decimal>
        {
            { "GEL", 1},
            { "USD", 2.65m },
            { "EUR", 2.73m }
        };

       

        public Task<decimal> ConvertCurrency(decimal amount, string senderAccountCurrency, string recipientAccountCurrency)
        {
            var sourceRate = _exchangeRates[recipientAccountCurrency];
            var targetRate = _exchangeRates[senderAccountCurrency];

            var convertedAmount = amount * (targetRate / sourceRate);

            return Task.FromResult(convertedAmount);
        }

        public decimal GetRate(string SenderAccountCurrency, string recipientAccountCurrency)
        {
            var sourceRate = _exchangeRates[recipientAccountCurrency];
            var targetRate = _exchangeRates[SenderAccountCurrency];

            var rate = targetRate / sourceRate;
            return rate;
        }

        public decimal GetDailyLimitForEachCurrency(decimal amountOfLimit, string limitCurrency, string senderAccountCurrency)
        {
            var sourceRate = _exchangeRates[limitCurrency];
            var targetRate = _exchangeRates[senderAccountCurrency];

            var convertedLimit = amountOfLimit * (targetRate / sourceRate);

            return convertedLimit;
        }
    }
}
