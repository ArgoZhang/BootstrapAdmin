using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bootstrap.Client.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public class GiteeHttpClient
    {
        HttpClient Client { get; set; }

        IConfiguration Configuration { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="client"></param>
        public GiteeHttpClient(IConfiguration configuration, HttpClient client)
        {
            Configuration = configuration;
            Client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<bool> Post(GiteeQueryBody query, GiteePushBody payload)
        {
            var ret = false;
            if (query.Id == "melhgtr0awltdhrh")
            {
                var allowBranchs = query.AllowBranchs.SpanSplit("|");
                var branch = payload.Ref.SpanSplit("/").LastOrDefault();
                if (!string.IsNullOrEmpty(branch) && allowBranchs.Any(b => b.Equals(branch, StringComparison.OrdinalIgnoreCase)))
                {
                    var accountName = Configuration["B4BIM:AccountName"];
                    var projectSlug = Configuration["B4BIM:ProjectSlug"];

                    // 调用 webhook 接口
                    // http://localhost:50852/api/Traces

                    var resp = await Client.PostAsJsonAsync("", new AppveyorBuildPostBody() { AccountName = accountName, ProjectSlug = projectSlug, Branch = branch });
                    ret = resp.IsSuccessStatusCode;
                }
            }
            return ret;
        }
    }
}
