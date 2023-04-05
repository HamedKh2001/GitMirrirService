﻿using GitMirrorService;
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

        public MirrorJob(ILogger<MirrorJob> logger, IConfiguration configuration)
        {
            _logger = logger;

            FrontEndInfo = configuration.GetSection("FrontEndInfo").Get<Info>();
            BackEndInfo = configuration.GetSection("BackEndInfo").Get<Info>();
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => Mirror.StarToMirror(BackEndInfo.SourceUrl, BackEndInfo.DestinationUrl, BackEndInfo.SaveDirectory, BackEndInfo.WaitForDownload));
            await Task.Run(() => Mirror.StarToMirror(FrontEndInfo.SourceUrl, FrontEndInfo.DestinationUrl, FrontEndInfo.SaveDirectory, FrontEndInfo.WaitForDownload));
            _logger.LogInformation("Mirror Done.");

            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Count Of Mirrors: {Count}", count);
        }
    }
}
