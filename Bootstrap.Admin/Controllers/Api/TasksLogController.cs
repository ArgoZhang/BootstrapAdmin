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
        /// 任务管理页面日志按钮调用此方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="hub"></param>
        /// <returns></returns>
        [HttpGet]
        public bool Get([FromQuery]string name, [FromServices]IHubContext<SignalRHub> hub)
        {
            var sche = TaskServicesManager.GetOrAdd(name);
            sche.Triggers.First().PulseCallback = t => SendTaskLog(sche, name, hub);
            SendTaskLog(sche, name, hub);
            return true;
        }

        private void SendTaskLog(IScheduler sche, string name, IHubContext<SignalRHub> hub)
        {
            var t = sche.Triggers.First();
            var result = $"{{\"name\": \"{name}\", \"msg\": \"{sche.LastRuntime}: Trigger({t.GetType().Name}) Run({t.LastResult}) NextRuntime: {sche.NextRuntime} Elapsed: {t.LastRunElapsedTime.Seconds}s\"}}";
            SignalRManager.SendTaskLog(hub.Clients.All, result).ConfigureAwait(false);
        }
    }
}
