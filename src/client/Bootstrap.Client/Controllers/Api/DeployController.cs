using Bootstrap.Client.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Client.Controllers.Api
{
    /// <summary>
    /// 自动发布 WebApi 接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DeployController : ControllerBase
    {
        /// <summary>
        /// 自动发布 webhook 接口
        /// </summary>
        /// <param name="args"></param>
        [HttpPost]
        public void Post([FromBody]GiteePushEventArgs args)
        {
            DeployTaskManager.Add(args);
        }
    }
}
