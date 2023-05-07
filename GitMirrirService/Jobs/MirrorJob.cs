using GitMirrorService;
using Quartz;

namespace BRTechGroup.JMS.HostedService.Jobs
{
    [DisallowConcurrentExecution]
    public class MirrorJob : IJob
    {
        private int executionCount = 0;
        private Info FrontEndInfo = new();
        private Info BackEndInfo = new();
        private readonly ILogger<MirrorJob> _logger;

        public MirrorJob(IConfiguration configuration, ILogger<MirrorJob> logger)
        {

            FrontEndInfo = configuration.GetSection("FrontEndInfo").Get<Info>();
            BackEndInfo = configuration.GetSection("BackEndInfo").Get<Info>();
            _logger=logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.Run(() => Mirror.StarToMirror(BackEndInfo.SourceUrl, BackEndInfo.DestinationUrl, BackEndInfo.SaveDirectory, _logger));
                await Task.Run(() => Mirror.StarToMirror(FrontEndInfo.SourceUrl, FrontEndInfo.DestinationUrl, FrontEndInfo.SaveDirectory, _logger));
                _logger.LogInformation("Mirror Done.");

                var count = Interlocked.Increment(ref executionCount);

                _logger.LogInformation(
                    "Count Of Mirrors: {Count}", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
