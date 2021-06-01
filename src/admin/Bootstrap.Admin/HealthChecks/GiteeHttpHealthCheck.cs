using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Bootstrap.Admin.HealthChecks
{
    /// <summary>
    /// Gitee 接口检查器
    /// </summary>
    public class GiteeHttpHealthCheck : IHealthCheck
    {
        private readonly GiteeHttpClient _client;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="accessor"></param>
        public GiteeHttpHealthCheck(GiteeHttpClient client, IHttpContextAccessor accessor)
        {
            _client = client;
            _client.HttpClient.BaseAddress = new Uri($"{accessor.HttpContext!.Request.Scheme}://{accessor.HttpContext?.Request.Host}{accessor.HttpContext?.Request.PathBase}");
        }

        /// <summary>
        /// 异步检查方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var urls = new string[] { "Issues", "Pulls", "Releases", "Builds" };
            var data = new Dictionary<string, object>();

            Task.WaitAll(urls.Select(url => Task.Run(async () =>
            {
                var sw = Stopwatch.StartNew();
                Exception? error = null;
                var result = await _client.HttpClient.GetFromJsonAsync<object>($"/api/Gitee/{url}", cancellationToken);
                sw.Stop();
                data.Add(url, error == null ? $"{result} Elapsed: {sw.Elapsed}" : $"{result} Elapsed: {sw.Elapsed} Exception: {error}");
            })).ToArray());
            return Task.FromResult(HealthCheckResult.Healthy("Ok", data));
        }
    }
}
