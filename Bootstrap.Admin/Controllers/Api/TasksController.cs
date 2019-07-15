using Longbow.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            TaskServicesManager.GetOrAdd("测试任务", token => Task.Delay(1000), TriggerBuilder.WithCronExpression("*/5 * * * * *"));
            return TaskServicesManager.ToList().Select(s => new { s.Name, s.Enabled, s.Status, s.LastRuntime, s.CreatedTime, s.NextRuntime, Triggers = s.Triggers.Count, s.LastRunResult, TriggerExpression = s.Triggers.FirstOrDefault().ToString() });
        }
    }
}
