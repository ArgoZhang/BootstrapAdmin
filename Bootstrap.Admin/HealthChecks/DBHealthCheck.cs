using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Http;
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
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly Func<IConfiguration, string, string> ConnectionStringResolve = (c, name) => string.IsNullOrEmpty(name)
            ? c.GetSection("ConnectionStrings").GetChildren().FirstOrDefault()?.Value
            : c.GetConnectionString(name);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="httpContextAccessor"></param>
        public DBHealthCheck(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

            // 检查 当前用户 账户权限
            var loginUser = _httpContextAccessor.HttpContext.User.Identity.Name;
            var userName = loginUser ?? "Admin";
            var user = UserHelper.RetrieveUserByUserName(userName);
            var roles = RoleHelper.RetrievesByUserName(userName) ?? new string[0];
            var menus = MenuHelper.RetrieveMenusByUserName(userName) ?? new BootstrapMenu[0];
            var dicts = DictHelper.RetrieveDicts() ?? new BootstrapDict[0];
            var assemblyLoaded = DbContextManager.Exception == null ? "Loaded" : "NotLoad";
            var data = new Dictionary<string, object>()
            {
                { "ConnectionString", db.ConnectionString },
                { "Widget", $"{db.Widget}({assemblyLoaded})" },
                { "Reference", DbContextManager.Exception?.Message },
                { "DbType", db.ProviderName },
                { "Dicts", dicts.Count() },
                { "LoginName", loginUser },
                { "DisplayName", user?.DisplayName },
                { "Roles", string.Join(",", roles) },
                { "Navigations", menus.Count() }
            };

            var v = dicts.Any() && user != null && roles.Any() && menus.Any() && DbContextManager.Exception == null;
            return v ? Task.FromResult(HealthCheckResult.Healthy("Ok", data)) : Task.FromResult(HealthCheckResult.Degraded("Failed", null, data));
        }
    }
}
