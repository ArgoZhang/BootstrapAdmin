﻿using Bootstrap.Client.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bootstrap.Client.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class GitController : ControllerBase
    {
        /// <summary>
        /// Appveyor 私有服务器 Webhook
        /// </summary>
        /// <param name="client"></param>
        /// <param name="query"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Webhook([FromServices] AppVeyorHttpClient client, [FromQuery] GiteeQueryBody query, [FromBody] WebhookPostBody payload) => new StatusCodeResult((int)await client.Post(query, payload));

        /// <summary>
        /// 跨域握手协议
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public string Options()
        {
            return string.Empty;
        }
    }
}
