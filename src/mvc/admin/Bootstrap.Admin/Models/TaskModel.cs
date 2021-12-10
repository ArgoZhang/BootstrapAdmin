using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 任务管理页面 Model 类
    /// </summary>
    public class TaskModel : NavigatorBarModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="controller"></param>
        public TaskModel(ControllerBase controller) : base(controller)
        {
            // 此处为演示代码，具体生产环境可以从数据库配置获得
            // Key 为任务名称 Value 为任务执行体 FullName
            TaskExecutors = new Dictionary<string, string>
            {
                { "测试任务", "Bootstrap.Admin.DefaultTaskExecutor" }
            };

            TaskTriggers = new Dictionary<string, string>
            {
                { "每 5 秒钟执行一次", Longbow.Tasks.Cron.Secondly(5) },
                { "每 1 分钟执行一次", Longbow.Tasks.Cron.Minutely(1) },
                { "每 5 分钟执行一次", Longbow.Tasks.Cron.Minutely(5) }
            };
        }

        /// <summary>
        /// 获得 系统内置的所有任务
        /// </summary>
        public IDictionary<string, string> TaskExecutors { get; }

        /// <summary>
        /// 获得 系统内置触发器集合
        /// </summary>
        /// <value></value>
        public IDictionary<string, string> TaskTriggers { get; }
    }
}
