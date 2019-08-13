using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

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
            using (var db = DbManager.Create())
            {
                var connStr = db.ConnectionString;
                var dicts = db.Fetch<BootstrapDict>("Select * from Dicts");
                return dicts.Any() ? Task.FromResult(HealthCheckResult.Healthy("Ok")) : Task.FromResult(HealthCheckResult.Degraded("No init data in DB"));
            }
        }
    }
}
