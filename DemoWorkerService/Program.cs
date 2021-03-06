using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DemoWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<WhenToRunOptions>(
                            hostContext
                                .Configuration
                                    .GetSection(WhenToRunOptions.WhenToRun));

                    services.AddHostedService<Worker>();
                });
    }
}
