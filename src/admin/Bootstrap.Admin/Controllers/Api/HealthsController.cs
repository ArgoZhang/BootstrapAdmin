using Bootstrap.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 健康检查控制器
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class HealthsController : ControllerBase
    {
        /// <summary>
        /// 发送健康检查结果
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Healths([FromServices] GiteeHttpClient httpClient, [FromServices] IConfiguration config, [FromBody] string message)
        {
            var ret = false;
            var url = config.GetValue("HealthsCloudUrl", "");
            if (!string.IsNullOrEmpty(url) && DictHelper.RetrieveHealth())
            {
                await httpClient.HttpClient.PostAsJsonAsync(url, message);
                ret = true;
            }
            return ret;
        }
    }
}
