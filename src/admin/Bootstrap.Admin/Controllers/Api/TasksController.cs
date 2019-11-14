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
            return true;
        }
    }
}
