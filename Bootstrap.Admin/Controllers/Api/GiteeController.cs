using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class GiteeController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public async Task<ActionResult> Issues([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "LongbowEnterprise", [FromQuery]string repoName = "BootstrapAdmin")
        {
            var client = httpClientFactory.CreateClient();
            var content = await client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/issues");
            var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
            var labels = new string[] { "open", "closed", "rejected" };
            var result = regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
            return new JsonResult(new { schemaVersion = 1, label = string.Join(" ", result), message = "sweet world", color = "orange" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public async Task<ActionResult> Pulls([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "LongbowEnterprise", [FromQuery]string repoName = "BootstrapAdmin")
        {
            var client = httpClientFactory.CreateClient();
            var content = await client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/pulls");
            var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
            var labels = new string[] { "open", "merged", "closed" };
            var result = regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
            return new JsonResult(new { schemaVersion = 1, label = string.Join(" ", result), message = "sweet world", color = "orange" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public async Task<ActionResult> Releases([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "LongbowEnterprise", [FromQuery]string repoName = "BootstrapAdmin")
        {
            var client = httpClientFactory.CreateClient();
            var content = await client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/releases");
            var regex = Regex.Match(content, $"<a href=\"/{userName}/{repoName}/releases/([^\\s]+)\" target=\"_blank\">", RegexOptions.IgnoreCase);
            var result = regex.Groups[1].Value;
            return new JsonResult(new { schemaVersion = 1, label = result, message = "sweet world", color = "orange" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userName"></param>
        /// <param name="projName"></param>
        /// <returns></returns>
        public async Task<ActionResult> Builds([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "ArgoZhang", [FromQuery]string projName = "bootstrapadmin")
        {
            var client = httpClientFactory.CreateClient();
            var content = await client.GetAsJsonAsync<AppveyorBuildResult>($"https://ci.appveyor.com/api/projects/{userName}/{projName}");
            return new JsonResult(new { schemaVersion = 1, label = content.Build.Version, message = "sweet world", color = "orange" });
        }

        /// <summary>
        /// 
        /// </summary>
        private class AppveyorBuildResult
        {
            /// <summary>
            /// 
            /// </summary>
            public Build Build { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Build
        {
            /// <summary>
            /// 
            /// </summary>
            public string Version { get; set; }
        }
    }
}
