using Longbow.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<object> Get()
        {
            TaskServicesManager.GetOrAdd("单次任务", token => Task.Delay(1000));
            TaskServicesManager.GetOrAdd("周期任务", token => Task.Delay(1000), TriggerBuilder.Build(Cron.Secondly(5)));
            TaskServicesManager.GetOrAdd("超时任务", token => Task.Delay(2000), TriggerBuilder.Default.WithTimeout(1000).WithInterval(1000).WithRepeatCount(2).Build());
            TaskServicesManager.GetOrAdd("故障任务", token => throw new Exception("故障任务"));
            TaskServicesManager.GetOrAdd("取消任务", token => Task.Delay(1000)).Triggers.First().Enabled = false;
            return TaskServicesManager.ToList().Select(s => new { s.Name, Status = s.Status.ToString(), s.LastRuntime, s.CreatedTime, s.NextRuntime, LastRunResult = s.Triggers.First().LastResult.ToString(), TriggerExpression = s.Triggers.FirstOrDefault().ToString() }).OrderBy(s => s.Name);
        }
    }
}
