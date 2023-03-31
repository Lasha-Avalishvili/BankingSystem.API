using BankingSystem.DB;
using BankingSystem.WorkerService.Services;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddDbContextPool<AppDbContext>(o => o.UseSqlServer(hostContext.Configuration.GetConnectionString("AppToDB")));
               services.AddHostedService<Worker>();
               services.AddTransient<IExchangeRatesFetcher, ExchangeRatesFetcher>();
           });
    }
}