using Longbow.Tasks;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksLogController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hub"></param>
        /// <returns></returns>
        [HttpGet]
        public bool Get([FromQuery]string name, [FromServices]IHubContext<SignalRHub> hub)
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
    }
}
