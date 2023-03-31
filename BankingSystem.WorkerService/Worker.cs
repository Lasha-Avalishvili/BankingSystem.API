using BankingSystem.DB;
using BankingSystem.WorkerService.Services;

namespace BankingSystem.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var fetcher = new ExchangeRatesFetcher(db);

                Console.WriteLine("Fetching....");
                await fetcher.UpdateRates();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}