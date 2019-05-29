using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Issues([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "LongbowEnterprise", [FromQuery]string repoName = "BootstrapAdmin", [FromQuery]string label = "custom badge", [FromQuery]string color = "orange")
        {
            var client = httpClientFactory.CreateClient();
            var content = await GetJsonAsync(() => client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/issues"));
            var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
            var labels = new string[] { "open", "closed", "rejected" };
            var result = string.IsNullOrEmpty(content) ? new string[] { "unknown" } : regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
            return new JsonResult(new { schemaVersion = 1, label, message = string.Join(" ", result), color });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Pulls([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "LongbowEnterprise", [FromQuery]string repoName = "BootstrapAdmin", [FromQuery]string label = "custom badge", [FromQuery]string color = "orange")
        {
            var client = httpClientFactory.CreateClient();
            var content = await GetJsonAsync(() => client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/pulls"));
            var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
            var labels = new string[] { "open", "merged", "closed" };
            var result = string.IsNullOrEmpty(content) ? new string[] { "unknown" } : regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
            return new JsonResult(new { schemaVersion = 1, label, message = string.Join(" ", result), color });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Releases([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "LongbowEnterprise", [FromQuery]string repoName = "BootstrapAdmin", [FromQuery]string label = "custom badge", [FromQuery]string color = "orange")
        {
            var client = httpClientFactory.CreateClient();
            var content = await GetJsonAsync(() => client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/releases"));
            var regex = Regex.Match(content, $"<a href=\"/{userName}/{repoName}/releases/([^\\s]+)\" target=\"_blank\">", RegexOptions.IgnoreCase);
            var result = string.IsNullOrEmpty(content) ? "unknown" : regex.Groups[1].Value;
            return new JsonResult(new { schemaVersion = 1, label, message = result, color });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userName"></param>
        /// <param name="projName"></param>
        /// <param name="branchName"></param>
        /// <param name="label"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Builds([FromServices]IHttpClientFactory httpClientFactory, [FromQuery]string userName = "ArgoZhang", [FromQuery]string projName = "bootstrapadmin", [FromQuery]string branchName = "master", [FromQuery]string label = "custom badge", [FromQuery]string color = "orange")
        {
            var client = httpClientFactory.CreateClient();
            var content = await GetJsonAsync(() => client.GetAsJsonAsync<AppveyorBuildResult>($"https://ci.appveyor.com/api/projects/{userName}/{projName}/branch/{branchName}"));
            return new JsonResult(new { schemaVersion = 1, label, message = content == null ? "unknown" : content.Build.Version, color });
        }

        private async static Task<T> GetJsonAsync<T>(Func<Task<T>> callback)
        {
            var ret = default(T);
            try
            {
                ret = await callback();
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                ex.Log();
            }
            return ret;
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
