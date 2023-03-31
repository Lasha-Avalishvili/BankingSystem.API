using BankingSystem.DB;
using BankingSystem.WorkerService.Services;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddHostedService<Worker>();
               services.AddTransient<IExchangeRatesFetcher, ExchangeRatesFetcher>();
           });
    }
}