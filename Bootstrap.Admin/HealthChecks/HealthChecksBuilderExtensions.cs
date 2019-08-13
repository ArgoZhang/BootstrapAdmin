using Bootstrap.Admin.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 健康检查扩展类
    /// </summary>
    public static class HealthChecksBuilderExtensions
    {
        /// <summary>
        /// 添加 BootstrapAdmin 健康检查
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddBootstrapAdminHealthChecks(this IHealthChecksBuilder builder)
        {
            builder.AddCheck<DBHealthCheck>("db");
            builder.AddCheck<FileHealCheck>("file");
            return builder;
        }
    }
}
