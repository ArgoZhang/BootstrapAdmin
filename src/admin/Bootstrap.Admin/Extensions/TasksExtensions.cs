using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Tasks;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading;
using Task = System.Threading.Tasks.Task;

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
            TaskServicesManager.GetOrAdd("周期任务", token => Task.Delay(1000), TriggerBuilder.Default.WithInterval(10000).Build());
            TaskServicesManager.GetOrAdd("Cron 任务", token => Task.Delay(1000), TriggerBuilder.Build(Cron.Secondly(5)));
            TaskServicesManager.GetOrAdd("超时任务", token => Task.Delay(2000), TriggerBuilder.Default.WithTimeout(1000).WithInterval(1000).WithRepeatCount(2).Build());

            // 本机调试时此处会抛出异常，配置文件中默认开启了任务持久化到物理文件，此处异常只有首次加载时会抛出
            // 此处异常是示例自定义任务内部未进行捕获异常时任务仍然能继续运行，不会导致整个进程崩溃退出
            // 此处代码可注释掉
            //TaskServicesManager.GetOrAdd("故障任务", token => throw new Exception("故障任务"));
            TaskServicesManager.GetOrAdd("取消任务", token => Task.Delay(1000)).Triggers.First().Enabled = false;

            // 创建任务并禁用
            TaskServicesManager.GetOrAdd("禁用任务", token => Task.Delay(1000)).Status = SchedulerStatus.Disabled;

            // 真实任务负责批次写入数据执行脚本到日志中
            TaskServicesManager.GetOrAdd<DBLogTask>("SQL日志", TriggerBuilder.Build(Cron.Minutely()));

            // 真实人物负责周期性设置健康检查结果开关为开启
            TaskServicesManager.GetOrAdd("健康检查", token => Task.FromResult(DictHelper.SaveSettings(new BootstrapDict[] {
                new BootstrapDict() {
                    Category = "网站设置",
                    Name = "健康检查",
                    Code = "1",
                    Define = 0
                }
            })), TriggerBuilder.Build(Cron.Minutely(10)));
        });
    }
}
