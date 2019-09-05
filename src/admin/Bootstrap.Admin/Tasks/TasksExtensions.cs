using Longbow.Tasks;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 后台任务扩展方法
    /// </summary>
    internal static class TasksExtensions
    {
        /// <summary>
        /// 添加示例后台任务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddBootstrapAdminBackgroundTask(this IServiceCollection services)
        {
            services.AddTaskServices(builder => builder.AddFileStorage());
            services.AddHostedService<BootstrapAdminBackgroundServices>();
            return services;
        }
    }

    /// <summary>
    /// 后台任务服务类
    /// </summary>
    internal class BootstrapAdminBackgroundServices : BackgroundService
    {
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
        {
            TaskServicesManager.GetOrAdd("单次任务", token => Task.Delay(1000));
            TaskServicesManager.GetOrAdd("周期任务", token => Task.Delay(1000), TriggerBuilder.Build(Cron.Secondly(5)));
            TaskServicesManager.GetOrAdd("超时任务", token => Task.Delay(2000), TriggerBuilder.Default.WithTimeout(1000).WithInterval(1000).WithRepeatCount(2).Build());
            TaskServicesManager.GetOrAdd("故障任务", token => throw new Exception("故障任务"));
            TaskServicesManager.GetOrAdd("取消任务", token => Task.Delay(1000)).Triggers.First().Enabled = false;
        });
    }
}
