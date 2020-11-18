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
        private static readonly Func<IConfiguration, string, string?> ConnectionStringResolve = (c, name) => string.IsNullOrEmpty(name)
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
                .Select(config => new DbOption()
                {
                    Enabled = bool.TryParse(config["Enabled"], out var en) ? en : false,
                    ProviderName = config["ProviderName"],
                    Widget = config["Widget"],
                    ConnectionString = ConnectionStringResolve(config.GetSection("ConnectionStrings").Exists() ? config : _configuration, string.Empty)
                }).FirstOrDefault(i => i.Enabled) ?? new DbOption()
                {
                    Enabled = true,
                    ProviderName = Longbow.Data.DatabaseProviderType.SqlServer.ToString(),
                    Widget = typeof(User).Assembly.FullName,
                    ConnectionString = Longbow.Data.DbManager.GetConnectionString()
                };

            // 检查 当前用户 账户权限
            var loginUser = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            var userName = loginUser ?? "Admin";
            var dictsCount = 0;
            var menusCount = 0;
            var roles = string.Empty;
            var displayName = string.Empty;
            var healths = false;
            Exception? error = null;
            try
            {
                var user = UserHelper.RetrieveUserByUserName(userName);
                displayName = user?.DisplayName ?? string.Empty;
                roles = string.Join(",", RoleHelper.RetrievesByUserName(userName) ?? new string[0]);
                menusCount = MenuHelper.RetrieveMenusByUserName(userName)?.Count() ?? 0;
                dictsCount = DictHelper.RetrieveDicts()?.Count() ?? 0;
                healths = user != null && !string.IsNullOrEmpty(roles) && menusCount > 0 && dictsCount > 0;

                // 检查数据库是否可写
                var dict = new BootstrapDict()
                {
                    Category = "DB-Check",
                    Name = "WriteTest",
                    Code = "1"
                };
                if (DictHelper.Save(dict) && !string.IsNullOrEmpty(dict.Id)) DictHelper.Delete(new string[] { dict.Id });
            }
            catch (Exception ex)
            {
                error = ex;
            }
            var data = new Dictionary<string, object>()
            {
                { "ConnectionString", db.ConnectionString ?? string.Empty },
                { "Reference", DbContextManager.Create<Dict>()?.GetType().Assembly.FullName ?? db.Widget ?? string.Empty },
                { "DbType", db?.ProviderName ?? string.Empty },
                { "Dicts", dictsCount },
                { "LoginName", userName },
                { "DisplayName", displayName },
                { "Roles", roles },
                { "Navigations", menusCount }
            };

            if (string.IsNullOrEmpty(db?.ConnectionString))
            {
                // 未启用连接字符串
                data["ConnectionString"] = "未配置数据库连接字符串";
                return Task.FromResult(HealthCheckResult.Unhealthy("Error", null, data));
            }

            if (DbContextManager.Exception != null) error = DbContextManager.Exception;
            if (error != null)
            {
                data.Add("Exception", error.Message);

                if (error.Message.Contains("SQLite Error 8: 'attempt to write a readonly database'.")) data.Add("解决办法", "更改数据库文件为可读，并授予进程可写权限");
                if (error.Message.Contains("Could not load", StringComparison.OrdinalIgnoreCase)) data.Add("解决办法", "Nuget 引用相对应的数据库驱动 dll");

                // UNDONE: Json 序列化循环引用导致异常 NET 5.0 修复此问题
                // 目前使用 new Exception() 临时解决
                return Task.FromResult(HealthCheckResult.Unhealthy("Error", new Exception(error.Message), data));
            }

            return healths ? Task.FromResult(HealthCheckResult.Healthy("Ok", data)) : Task.FromResult(HealthCheckResult.Degraded("Failed", null, data));
        }

        private class DbOption
        {
            public bool Enabled { get; set; }
            public string? ProviderName { get; set; }
            public string? Widget { get; set; }
            public string? ConnectionString { get; set; }
        }
    }
}
