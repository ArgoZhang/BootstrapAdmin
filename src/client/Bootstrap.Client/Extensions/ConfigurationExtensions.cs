using Microsoft.AspNetCore;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// IConfiguration 扩展类
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 获得 配置文件中 BootstrapAdminAuthenticationOptions 实例
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static BootstrapAdminAuthenticationOptions GetBootstrapAdminAuthenticationOptions(this IConfiguration configuration) => configuration.GetSection<BootstrapAdminAuthenticationOptions>().Get<BootstrapAdminAuthenticationOptions>();
    }
}
