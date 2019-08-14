using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bootstrap.Admin.HealthChecks
{
    /// <summary>
    /// Gitee 接口检查器
    /// </summary>
    public class GiteeHttpHealthCheck : IHealthCheck
    {
        private readonly HttpClient _client;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="accessor"></param>
        public GiteeHttpHealthCheck(IHttpClientFactory factory, IHttpContextAccessor accessor)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri($"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host}{accessor.HttpContext.Request.PathBase}");
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

            urls.ToList().ForEach(url =>
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    var task = _client.GetStringAsync($"/api/Gitee/{url}");
                    task.Wait(cancellationToken);
                }
                catch (Exception)
                {

                }
                finally
                {
                    sw.Stop();
                    data.Add(url, sw.Elapsed);
                }
            });
            return Task.FromResult(HealthCheckResult.Healthy("Ok", data));
        }
    }
}
