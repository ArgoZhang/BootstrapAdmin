using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Bootstrap.Admin.HealthChecks
{
    /// <summary>
    /// 内存检查器
    /// </summary>
    public class MemoryHealthCheck : IHealthCheck
    {
        /// <summary>
        /// 异步检查方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var process = Process.GetCurrentProcess();
            var data = new Dictionary<string, object>()
            {
                { "Id", process.Id },
                { "WorkingSet", process.WorkingSet64  },
                { "PrivateMemory", process.PrivateMemorySize64 },
                { "VirtualMemory", process.VirtualMemorySize64 },
            };

            return Task.FromResult(HealthCheckResult.Healthy("Ok", data));
        }
    }
}
