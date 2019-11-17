using Microsoft.Extensions.Configuration;

namespace Bootstrap.Admin.Pages
{
    /// <summary>
    /// 配置文件静态操作类
    /// </summary>
    public static class ConfigurationHelperExtensions
    {
        /// <summary>
        /// 获得本站 AppId
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetAppId(this IConfiguration configuration) => configuration.GetValue("AppId", "BA");
    }
}
