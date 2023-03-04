using BRTechGroup.JMS.HostedService.HostedServices;
using BRTechGroup.JMS.HostedService.JobFactories;
using BRTechGroup.JMS.HostedService.Jobs;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;

namespace GitMirrorService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            builder.Services.AddSingleton<MirrorJob>();
            builder.Services.AddSingleton(new JobSchedule(jobType: typeof(MirrorJob), cronExpression:
                builder.Configuration["Quartz:MirrorCronExpression"]
            ));
            builder.Services.AddHostedService<QuartzHostedService>();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(serviceType: typeof(Nlogger.ILogger<>), implementationType: typeof(Nlogger.NLogAdapter<>));
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}