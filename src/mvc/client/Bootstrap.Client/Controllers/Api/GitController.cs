using Bootstrap.Client.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
        /// 通过包名称获得下一个版本号信息
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="repoName">包名称</param>
        /// <param name="branchName">分支名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> NugetNextVersion([FromServices] IHttpClientFactory httpClientFactory, [FromQuery] string? repoName = "bootstrapblazor", [FromQuery] string? branchName = "dev")
        {
            var url = "https://nuget.cdn.azure.cn/v3-flatcontainer/bootstrapblazor/index.json";
            using var client = httpClientFactory.CreateClient();
            var versions = await client.GetFromJsonAsync<PackageVersion>(url);
            var currentVersion = versions?.Versions.Last();
            var version = "";
            if (currentVersion != null)
            {
                // dev 分支发布 beta 版本
                // master 分支发布正式版
                if (Version.TryParse(currentVersion, out var v))
                {

                }
                if (branchName == "dev")
                {

                }
            }
            return version;
        }

        /// <summary>
        /// 跨域握手协议
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public string Options()
        {
            return string.Empty;
        }

        private class PackageVersion
        {
            [NotNull]
            public IEnumerable<string>? Versions { get; set; }
        }

        private class BlazorVersion
        {
            public int Minor { get; }
            public int Major { get; }
            public int Build { get; }
            public int Revision { get; }
        }
    }
}
