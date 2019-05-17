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
        /// <returns></returns>
        public async Task<ActionResult> Issues([FromServices]IHttpClientFactory httpClientFactory)
        {
            var client = httpClientFactory.CreateClient();
            var content = await client.GetStringAsync("https://gitee.com/LongbowEnterprise/BootstrapAdmin/issues");
            var regex = Regex.Matches(content, "<div class='ui mini circular label'>([\\d]+)</div>", RegexOptions.IgnoreCase);
            var labels = new string[] { "open", "closed", "rejected" };
            var result = regex.Select((m, i) => $"{labels[i]} {m.Groups[1].Value}");
            return new JsonResult(new { schemaVersion = 1, label = string.Join(" ", result), message = "sweet world", color = "orange" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <returns></returns>
        public async Task<ActionResult> Releases([FromServices]IHttpClientFactory httpClientFactory)
        {
            var client = httpClientFactory.CreateClient();
            var content = await client.GetStringAsync("https://gitee.com/LongbowEnterprise/BootstrapAdmin/releases");
            var regex = Regex.Match(content, "<a href=\"/LongbowEnterprise/BootstrapAdmin/releases/([^\\s]+)\" target=\"_blank\">", RegexOptions.IgnoreCase);
            var result = regex.Groups[1].Value;
            return new JsonResult(new { schemaVersion = 1, label = result, message = "sweet world", color = "orange" });
        }
    }
}
