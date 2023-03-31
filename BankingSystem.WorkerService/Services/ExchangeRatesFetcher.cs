using BankingSystem.DB;
using BankingSystem.DB.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

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

            foreach (var currency in currencies)
            {
                var newCurrency = new ExchangeRateEntity
                {
                    QuoteCurrency = currency.code,
                    Rate = currency.rateFormated
                };
                _db.ExchangeRates.Add(newCurrency);
            }
            await _db.SaveChangesAsync();
        }
    }
}
