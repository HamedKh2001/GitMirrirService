namespace GitMirrorService
{
    public class MirrorService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<MirrorService> _logger;
        private Timer _timer = null;
        private TimeSpan timeout;

        public MirrorService(ILogger<MirrorService> logger, IConfiguration configuration)
        {
            _logger = logger;

            var hours = Convert.ToInt32(configuration.GetSection("PeriodTime")["Hour"]);
            var minutes = Convert.ToInt32(configuration.GetSection("PeriodTime")["Minutes"]);
            var seconds = Convert.ToInt32(configuration.GetSection("PeriodTime")["Seconds"]);
            timeout = new TimeSpan(hours, minutes, seconds);
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted Service Started To Mirroring.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, timeout);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object state)
        {
            Mirror.StarToMirror();
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Count Of Mirrors: {Count}", count);
        }
    }
}
