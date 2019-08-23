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
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdminHealthChecks(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<DBHealthCheck>("db");
            builder.AddBootstrapAdminHealthChecks();
            builder.AddCheck<GiteeHttpHealthCheck>("Gitee");
            return services;
        }
    }
}
