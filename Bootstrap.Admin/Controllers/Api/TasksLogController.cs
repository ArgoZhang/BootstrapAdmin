using Longbow.Tasks;
using Longbow.Web.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

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
            sche.Triggers.First().PulseCallback = async t =>
            {
                var result = $"{{\"name\": \"{name}\", \"msg\": \"{sche.LastRuntime}: Trigger({t.GetType().Name}) Run({t.LastResult}) NextRuntime: {sche.NextRuntime} Elapsed: {t.LastRunElapsedTime.Seconds}s\"}}";
                await SignalRManager.SendTaskLog(hub.Clients.All, result);
            };
            return true;
        }
    }
}
