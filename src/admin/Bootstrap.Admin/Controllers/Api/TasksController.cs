using Bootstrap.DataAccess;
using Longbow;
using Longbow.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            return TaskServicesManager.ToList().Select(s => new { s.Name, Status = s.Status.ToString(), s.LastRuntime, s.CreatedTime, s.NextRuntime, LastRunResult = s.Triggers.First().LastResult.ToString(), TriggerExpression = s.Triggers.First().ToString() }).OrderBy(s => s.Name);
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

            // 系统内置任务禁止更改
            // 演示模式下禁止删除内置任务
            if (DictHelper.RetrieveSystemModel() && _tasks.Any(t => t.Equals(widget.Name, StringComparison.OrdinalIgnoreCase))) return false;

            // 加载任务执行体
            // 此处可以扩展为任意 DLL 中的任意继承 ITask 接口的实体类
            var taskExecutor = LgbActivator.CreateInstance<ITask>("Bootstrap.Admin", widget.TaskExecutorName);
            if (taskExecutor == null) return false;

            // 此处未存储到数据库中，直接送入任务中心
            TaskServicesManager.Remove(widget.Name);
            TaskServicesManager.GetOrAdd(widget.Name, token => taskExecutor.Execute(token), TriggerBuilder.Build(widget.CronExpression));
            return true;
        }

        private static IEnumerable<string> _tasks = new string[] {
            "单次任务",
            "周期任务",
            "Cron 任务",
            "超时任务",
            "取消任务",
            "禁用任务",
            "SQL日志",
            "健康检查"
        };
        /// <summary>
        /// 删除任务方法
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool Delete([FromBody]IEnumerable<string> ids)
        {
            // 演示模式下禁止删除内置任务
            if (DictHelper.RetrieveSystemModel() && _tasks.Any(t => ids.Any(id => id.Equals(t, StringComparison.OrdinalIgnoreCase)))) return false;

            // 循环删除任务
            ids.ToList().ForEach(id => TaskServicesManager.Remove(id));
            return true;
        }
    }
}
