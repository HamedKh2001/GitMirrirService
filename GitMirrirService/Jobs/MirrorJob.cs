using GitMirrorService;
using Quartz;

namespace BRTechGroup.JMS.HostedService.Jobs
{
    [DisallowConcurrentExecution]
    public class MirrorJob : IJob
    {
        private int executionCount = 0;
        private string SourceUrl = string.Empty;
        private string DestinationUrl = string.Empty;
        private readonly ILogger<MirrorJob> _logger;
        private Timer _timer = null;
        private TimeSpan timeout;

        public MirrorJob(ILogger<MirrorJob> logger, IConfiguration configuration)
        {
            _logger = logger;

            SourceUrl = configuration.GetSection("SourceUrl").Value;
            DestinationUrl = configuration.GetSection("DestinationUrl").Value;

            var hours = Convert.ToInt32(configuration.GetSection("PeriodTime")["Hour"]);
            var minutes = Convert.ToInt32(configuration.GetSection("PeriodTime")["Minutes"]);
            var seconds = Convert.ToInt32(configuration.GetSection("PeriodTime")["Seconds"]);
            timeout = new TimeSpan(hours, minutes, seconds);
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => Mirror.StarToMirror(SourceUrl, DestinationUrl));
            _logger.LogInformation("Mirror Done.");

            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Count Of Mirrors: {Count}", count);
        }
    }
}
