using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bootstrap.Client.Extensions
{
    /// <summary>
    /// 缓存配置类
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    internal class SmtpConfigureOptions<TOptions> : ConfigureFromConfigurationOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public SmtpConfigureOptions(IConfiguration configuration)
            : base(configuration.GetSection("SmtpClient"))
        {

        }
    }
}
