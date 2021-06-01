using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bootstrap.Client.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public class AppVeyorHttpClient
    {
        HttpClient Client { get; set; }

        IConfiguration Configuration { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="client"></param>
        public AppVeyorHttpClient(IConfiguration configuration, HttpClient client)
        {
            Configuration = configuration;
            Client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<HttpStatusCode> Post(GiteeQueryBody query, WebhookPostBody payload)
        {
            var ret = HttpStatusCode.NoContent;
            var section = Configuration.GetSection($"Appveyor:{query.Id}");
            if (section != null)
            {
                var token = section["Token"];
                var url = section["Api"];
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Client.BaseAddress = new Uri(url);

                var allowBranchs = query.AllowBranchs.SpanSplit("|");
                var branch = payload.Ref.SpanSplit("/").LastOrDefault();
                if (!string.IsNullOrEmpty(branch) && allowBranchs.Any(b => b.Equals(branch, StringComparison.OrdinalIgnoreCase)))
                {
                    var accountName = section["AccountName"];
                    var projectSlug = section["ProjectSlug"];

                    // 调用 webhook 接口
                    // http://nugetp.b4bim.cn:8050/api/builds

                    var resp = await Client.PostAsJsonAsync("", new AppveyorBuildPostBody()
                    {
                        AccountName = accountName,
                        ProjectSlug = projectSlug,
                        Branch = branch
                    });
                    ret = resp.IsSuccessStatusCode ? HttpStatusCode.OK : resp.StatusCode;
                }
            }
            return ret;
        }
    }
}
