using Bootstrap.DataAccess;
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
                }).FirstOrDefault(i => i.Enabled) ?? new
                {
                    Enabled = true,
                    ProviderName = Longbow.Data.DatabaseProviderType.SqlServer.ToString(),
                    Widget = typeof(User).Assembly.FullName,
                    ConnectionString = Longbow.Data.DbManager.GetConnectionString()
                };

            // 检查 当前用户 账户权限
            var loginUser = _httpContextAccessor.HttpContext.User.Identity.Name;
            var userName = loginUser ?? "Admin";
            var dictsCount = 0;
            var menusCount = 0;
            var roles = string.Empty;
            var displayName = string.Empty;
            var healths = false;
            Exception error = null;
            try
            {
                DbContextManager.Exception = null;
                var user = UserHelper.RetrieveUserByUserName(userName);
                displayName = user?.DisplayName;
                roles = string.Join(",", RoleHelper.RetrievesByUserName(userName) ?? new string[0]);
                menusCount = MenuHelper.RetrieveMenusByUserName(userName)?.Count() ?? 0;
                dictsCount = DictHelper.RetrieveDicts()?.Count() ?? 0;
                healths = user != null && !string.IsNullOrEmpty(roles) && menusCount > 0 && dictsCount > 0;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            var data = new Dictionary<string, object>()
            {
                { "ConnectionString", db?.ConnectionString ?? "未配置数据库连接字符串" },
                { "Reference", DbContextManager.Create<Dict>()?.GetType().Assembly.FullName ?? db.Widget },
                { "DbType", db?.ProviderName },
                { "Dicts", dictsCount },
                { "LoginName", loginUser },
                { "DisplayName", displayName },
                { "Roles", roles },
                { "Navigations", menusCount }
            };

            if (db == null)
            {
                // 未启用连接字符串
                return Task.FromResult(HealthCheckResult.Unhealthy("Error", error ?? DbContextManager.Exception, data));
            }

            if (error != null || DbContextManager.Exception != null)
            {
                data.Add("Exception", (DbContextManager.Exception ?? error).Message);
                return Task.FromResult(HealthCheckResult.Unhealthy("Error", error ?? DbContextManager.Exception, data));
            }

            return healths ? Task.FromResult(HealthCheckResult.Healthy("Ok", data)) : Task.FromResult(HealthCheckResult.Degraded("Failed", null, data));
        }
    }
}
