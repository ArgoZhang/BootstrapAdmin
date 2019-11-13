using Longbow.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 任务日志控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
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
        public ActionResult Get([FromQuery]string name, [FromServices]IHubContext<TaskLogHub> hub)
        {
            var sche = TaskServicesManager.Get(name);
            var ret = false;
            if (sche != null)
            {
                sche.Triggers.First().PulseCallback = t => SendTaskLog(sche, name, hub);
                SendTaskLog(sche, name, hub);
                ret = true;
            }
            return Ok(ret);
        }

        private Task SendTaskLog(IScheduler sche, string name, IHubContext<TaskLogHub> hub)
        {
            var t = sche.Triggers.First();
            var result = $"{{\"name\": \"{name}\", \"msg\": \"Trigger({t.GetType().Name}) LastRuntime: {sche.LastRuntime?.ToString() ?? "none"} Run({t.LastResult}) NextRuntime: {sche.NextRuntime?.ToString() ?? "none"} Elapsed: {t.LastRunElapsedTime.Seconds}s\"}}";
            return hub.SendTaskLog(result);
        }
    }
}
