using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bootstrap.Admin.HealthChecks
{
    /// <summary>
    /// 数据库检查
    /// </summary>
    public class DBHealthCheck : IHealthCheck
    {
        /// <summary>
        /// 异步检查方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var data = new Dictionary<string, object>();
            data.Add("Test1", "Test1");
            data.Add("Test2", "Test2");
            return Task.FromResult(HealthCheckResult.Healthy("Ok", data));
        }
    }
}
