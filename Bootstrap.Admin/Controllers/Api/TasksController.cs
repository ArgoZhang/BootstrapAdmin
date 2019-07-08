using Longbow.Tasks;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool Post()
        {
            // UNDONE: 待完善
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hub"></param>
        /// <returns></returns>
        [HttpPut]
        public bool Put([FromQuery]string name, [FromServices]IHubContext<SignalRHub> hub)
        {
            var sche = TaskServicesManager.GetOrAdd(name);
            sche.Triggers[0].RegisterPulseCallback(async t =>
            {
                var success = t.Cancelled ? "Cancelled" : "Success";
                var result = $"{t.Scheduler.LastRuntime.Value.DateTime}: Trigger({t.GetType().Name}) Run({success}) NextRuntime: {t.NextRuntime.Value.DateTime} Elapsed: {t.LastRunElapsedTime.Seconds}s";
                await SignalRManager.SendTaskLog(hub.Clients.All, result);
            });
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public bool Delete() => true;
    }
}
