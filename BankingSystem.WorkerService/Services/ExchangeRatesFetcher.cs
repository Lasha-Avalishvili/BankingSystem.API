
using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Newtonsoft.Json;

namespace BankingSystem.WorkerService.Services
{
    public interface IExchangeRatesFetcher
    {
        Task UpdateRates();
    }
    public class ExchangeRatesFetcher : IExchangeRatesFetcher
    {
        private readonly AppDbContext _db;

        public ExchangeRatesFetcher(AppDbContext db)
        {
            _db = db;
        }

        public async Task UpdateRates()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/json");
            var json = await response.Content.ReadAsStringAsync();

            var currencyRates = JsonConvert.DeserializeObject<dynamic>(json);
            var currencies = currencyRates[0].currencies;

            var existingRates = _db.ExchangeRates.ToDictionary(r => r.QuoteCurrency);

            foreach (var currency in currencies)
            {
                var quoteCurrency = (string)currency.code;
                var rate = currency.rateFormated;
                if (existingRates.ContainsKey(quoteCurrency))
                {
                    // If the rate already exists, update it
                    var existingRate = existingRates[quoteCurrency];
                    existingRate.Rate = rate;
                    _db.ExchangeRates.Update(existingRate);
                }
                else
                {
                    // If the rate does not exist, add it
                    var newCurrency = new ExchangeRateEntity
                    {
                        QuoteCurrency = quoteCurrency,
                        Rate = rate
                    };
                    _db.ExchangeRates.Add(newCurrency);
                }
            }
            await _db.SaveChangesAsync();
        }
    }
}
