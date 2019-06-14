using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
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
            client.Timeout = TimeSpan.FromMilliseconds(2000);
            var ret = await GetJsonAsync(() => client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/issues"), content =>
            {
                var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
                var labels = new string[] { "open", "closed", "rejected" };
                var result = string.IsNullOrEmpty(content) ? new string[] { "unknown" } : regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
                return string.Join(" ", result);
            });
            return new JsonResult(new { schemaVersion = 1, label, message = ret, color });
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
            client.Timeout = TimeSpan.FromMilliseconds(2000);
            var ret = await GetJsonAsync(() => client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/pulls"), content =>
            {
                var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
                var labels = new string[] { "open", "merged", "closed" };
                var result = string.IsNullOrEmpty(content) ? new string[] { "unknown" } : regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
                return string.Join(" ", result);
            });
            return new JsonResult(new { schemaVersion = 1, label, message = ret, color });
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
            client.Timeout = TimeSpan.FromMilliseconds(2000);
            var ret = await GetJsonAsync(() => client.GetStringAsync($"https://gitee.com/{userName}/{repoName}/releases"), content =>
            {
                var regex = Regex.Match(content, $"<a href=\"/{userName}/{repoName}/releases/([^\\s]+)\" target=\"_blank\">", RegexOptions.IgnoreCase);
                return string.IsNullOrEmpty(content) ? "unknown" : regex.Groups[1].Value;
            });
            return new JsonResult(new { schemaVersion = 1, label, message = ret, color });
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
            var ret = await GetJsonAsync(() => client.GetAsJsonAsync<AppveyorBuildResult>($"https://ci.appveyor.com/api/projects/{userName}/{projName}/branch/{branchName}", null, new CancellationTokenSource(2000).Token), content =>
            {
                return content == null ? "unknown" : content.Build.Version;
            });
            return new JsonResult(new { schemaVersion = 1, label, message = ret, color });
        }

        private async static Task<string> GetJsonAsync<T>(Func<Task<T>> requestUrl, Func<T, string> callback)
        {
            var ret = "unresponsive";
            try
            {
                var resq = await requestUrl();
                ret = callback(resq);
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
