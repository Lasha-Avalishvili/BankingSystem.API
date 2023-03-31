using BankingSystem.WorkerService.Services;

namespace BankingSystem.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ExchangeRatesFetcher _ratesFetcher;

        public Worker(ExchangeRatesFetcher ratesFetcher)
        {
            _ratesFetcher = ratesFetcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Fetching....");
                await _ratesFetcher.UpdateRates();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}