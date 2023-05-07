using BRTechGroup.JMS.HostedService.HostedServices;
using BRTechGroup.JMS.HostedService.JobFactories;
using BRTechGroup.JMS.HostedService.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SharedKernel.Extensions;
using System.Diagnostics;


var webApplicationOptions = new WebApplicationOptions()
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args,
    ApplicationName = Process.GetCurrentProcess().ProcessName
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

builder.Host.AddMySerilogWithELK("GMS");
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<MirrorJob>();
builder.Services.AddSingleton(new JobSchedule(jobType: typeof(MirrorJob), cronExpression:
    builder.Configuration["Quartz:MirrorCronExpression"]
));
builder.Services.AddHostedService<QuartzHostedService>();

builder.Host.UseWindowsService();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
