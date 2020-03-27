using Bootstrap.Client.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class SendMailServicesCollectionExtensions
    {
        /// <summary>
        /// 添加邮件发送服务到容器中
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSendMail(this IServiceCollection services)
        {
            services.AddSingleton<IConfigureOptions<SmtpOption>, SmtpConfigureOptions<SmtpOption>>();
            services.AddSingleton<IOptionsChangeTokenSource<SmtpOption>, ConfigurationChangeTokenSource<SmtpOption>>();
            services.AddSingleton<ISendMail, DefaultSendMail>();
            return services;
        }
    }
}
