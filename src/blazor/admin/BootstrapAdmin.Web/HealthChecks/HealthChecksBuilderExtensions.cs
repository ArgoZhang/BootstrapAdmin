using BootstrapAdmin.Web.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 健康检查扩展类
/// </summary>
static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// 添加 BootstrapAdmin 健康检查
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAdminHealthChecks(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient<GiteeHttpClient>();

        var builder = services.AddHealthChecks();
        builder.AddCheck<DBHealthCheck>("db");
        builder.AddBootstrapAdminHealthChecks();
        builder.AddCheck<GiteeHttpHealthCheck>("Gitee");
        return services;
    }
}
