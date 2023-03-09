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
        private readonly IConfiguration _configuration;

        public MirrorJob(ILogger<MirrorJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            SourceUrl = configuration.GetSection("SourceUrl").Value;
            DestinationUrl = configuration.GetSection("DestinationUrl").Value;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => Mirror.StarToMirror(SourceUrl, DestinationUrl, _configuration["SaveDirectory"]));
            _logger.LogInformation("Mirror Done.");

            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Count Of Mirrors: {Count}", count);
        }
    }
}
