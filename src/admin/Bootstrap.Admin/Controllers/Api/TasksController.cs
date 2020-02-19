using Bootstrap.DataAccess;
using Longbow;
using Longbow.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 任务管理控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TasksController : ControllerBase
    {
        /// <summary>
        /// 获取所有任务数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<object> Get()
        {
            return TaskServicesManager.ToList().Select(s => new { s.Name, Status = s.Status.ToString(), s.LastRuntime, s.CreatedTime, s.NextRuntime, LastRunResult = s.Triggers.First().LastResult.ToString(), TriggerExpression = s.Triggers.FirstOrDefault().ToString() }).OrderBy(s => s.Name);
        }

        /// <summary>
        /// 任务相关操作
        /// </summary>
        /// <param name="scheName">调度名称</param>
        /// <param name="operType">操作方式 pause run</param>
        [HttpPut("{scheName}")]
        public bool Put(string scheName, [FromQuery]string operType)
        {
            var sche = TaskServicesManager.Get(scheName);
            if (sche != null) sche.Status = operType == "pause" ? SchedulerStatus.Disabled : SchedulerStatus.Running;

            // SQL 日志任务特殊处理
            if (scheName == "SQL日志")
            {
                if (operType == "pause") DBLogTask.Pause();
                else DBLogTask.Run();
            }
            return true;
        }

        /// <summary>
        /// 保存任务方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromBody]TaskWidget widget)
        {
            // 判断 Cron 表达式
            if (string.IsNullOrEmpty(widget.CronExpression)) return false;
            // 判断 任务是否已经存在
            if (TaskServicesManager.Get(widget.Name) != null) return false;

            // 加载任务执行体
            // 此处可以扩展为任意 DLL 中的任意继承 ITask 接口的实体类
            var taskExecutor = LgbActivator.CreateInstance<ITask>("Bootstrap.Admin", widget.TaskExecutorName);
            if (taskExecutor == null) return false;

            // 此处未存储到数据库中，直接送入任务中心
            var expression = Longbow.Tasks.Cron.ParseCronExpression(widget.CronExpression);
            TaskServicesManager.GetOrAdd(widget.Name, token => taskExecutor.Execute(token), TriggerBuilder.Build(widget.CronExpression));
            return true;
        }
    }
}
