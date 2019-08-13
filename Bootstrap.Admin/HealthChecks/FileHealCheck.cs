using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bootstrap.Admin.HealthChecks
{
    /// <summary>
    /// 文件健康检查类
    /// </summary>
    public class FileHealCheck : IHealthCheck
    {
        private readonly IHostingEnvironment _env;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        public FileHealCheck(IHostingEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// 异步检查方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var file = _env.IsDevelopment() ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Longbow.lic") : Path.Combine(_env.ContentRootPath, "Longbow.lic");
            var data = new Dictionary<string, object>();
            data.Add("ContentRootPath", _env.ContentRootPath);
            data.Add("WebRootPath", _env.WebRootPath);
            data.Add("ApplicationName", _env.ApplicationName);
            data.Add("EnvironmentName", _env.EnvironmentName);
            data.Add("CheckFile", file);
            return Task.FromResult(File.Exists(file) ? HealthCheckResult.Healthy("Ok", data) : HealthCheckResult.Unhealthy($"Missing file {file}", null, data));
        }
    }
}
