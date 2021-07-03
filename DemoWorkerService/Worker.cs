using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DemoWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;   
        private readonly WhenToRunOptions _options;

        public Worker(ILogger<Worker> logger,
            IOptions<WhenToRunOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogWarning($"Aplication DemoWorkerService started.");
                await StartAt(cancellationToken);
                await base.StartAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                //ignore StopAsync will be called
            }
            catch (Exception exception)
            {
                _logger.LogError($"Got exception on start up." +
                    $"Exception is {exception.Message}. Stopping service.");
                await StopAsync(cancellationToken);
            }            
        }

        private async Task StartAt(CancellationToken cancellationToken)
        {
            var timeToRun = new DateTimeOffset(
                DateTimeOffset.Now.Year,
                DateTimeOffset.Now.Month,
                DateTimeOffset.Now.Day,
                _options.Hours,
                _options.Minutes,
                _options.Seconds,
                0,
                DateTimeOffset.Now.Offset);

            int timeToSleep = GetTimeToSleep(timeToRun);

            _logger.LogWarning($"Aplication DemoWorkerService started" +
                $" and will sleep for {timeToSleep} milliseconds.");

            await Task.Delay(timeToSleep, cancellationToken);
        }

        private static int GetTimeToSleep(DateTimeOffset timeToRun)
        {
            var now = DateTimeOffset.Now;
            var timespan = timeToRun - now;
            int timeToSleep;

            if (timespan < TimeSpan.Zero)
            {
                var time = TimeSpan.FromHours(24) + timespan;
                timeToSleep = Convert.ToInt32(time.TotalMilliseconds);
            }
            else
            {
                timeToSleep = Convert.ToInt32(timespan.TotalMilliseconds);
            }

            return timeToSleep;
        }

        protected override async Task ExecuteAsync(
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");
                await Task.Delay(1000, cancellationToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}