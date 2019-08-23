using Bootstrap.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Bootstrap.Admin.HealthChecks
{
    /// <summary>
    /// 数据库检查类
    /// </summary>
    public class DBHealthCheck : IHealthCheck
    {
        private IConfiguration _configuration;
        private static readonly Func<IConfiguration, string, string> ConnectionStringResolve = (c, name) => string.IsNullOrEmpty(name)
            ? c.GetSection("ConnectionStrings").GetChildren().FirstOrDefault()?.Value
            : c.GetConnectionString(name);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public DBHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 异步检查方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var db = _configuration.GetSection("DB").GetChildren()
                .Select(config => new
                {
                    Enabled = bool.TryParse(config["Enabled"], out var en) ? en : false,
                    ProviderName = config["ProviderName"],
                    Widget = config["Widget"],
                    ConnectionString = ConnectionStringResolve(config.GetSection("ConnectionStrings").Exists() ? config : _configuration, string.Empty)
                }).FirstOrDefault(i => i.Enabled);

            // 检查 Admin 账户权限
            var user = UserHelper.RetrieveUserByUserName("Admin");
            var roles = RoleHelper.RetrievesByUserName("Admin");
            var dicts = DictHelper.RetrieveDicts();
            var menus = MenuHelper.RetrieveMenusByUserName("Admin");

            var data = new Dictionary<string, object>()
            {
                { "ConnectionString", db.ConnectionString },
                { "Widget", db.Widget },
                { "DbType", db.ProviderName },
                { "Dicts", dicts.Count() },
                { "User(Admin)", user != null },
                { "Roles(Admin)", string.Join(",", roles) },
                { "Navigations(Admin)", menus.Count() }
            };

            var v = dicts.Any() && user != null && roles.Any() && menus.Any();
            return v ? Task.FromResult(HealthCheckResult.Healthy("Ok", data)) : Task.FromResult(HealthCheckResult.Degraded("Failed"));
        }
    }
}
