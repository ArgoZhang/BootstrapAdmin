using Bootstrap.Client.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 自动发布服务扩展操作类
    /// </summary>
    public static class DeployExtensions
    {
        /// <summary>
        /// 注入自动发布到容器内
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoPublish(this IServiceCollection services)
        {
            DeployTaskManager.RegisterServices(services);
            return services;
        }
    }
}
