using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// BootstrapAdmin 健康检查扩展类
    /// </summary>
    public static class HealthChecksAppBuilderExtensions
    {
        /// <summary>
        /// 启用健康检查
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseBootstrapHealthChecks(this IApplicationBuilder app, PathString path = default)
        {
            if (path == default) path = "/Healths";
            app.UseHealthChecks(path, new HealthCheckOptions()
            {
                ResponseWriter = (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(report));
                },
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status200OK
                }
            });
            return app;
        }
    }
}
