using Bootstrap.Client.DataAccess;
using Bootstrap.Client.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http.Headers;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// HttpClient 扩展类
    /// </summary>
    internal static class HttpClientExtensions
    {
        /// <summary>
        /// 注入 TraceHttpClient 到容器中
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBootstrapHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<TraceHttpClient>((provider, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Connection.Add("keep-alive");

                // set auth
                var context = provider.GetRequiredService<IHttpContextAccessor>();
                var cookieValues = context.HttpContext!.Request.Cookies.Select(cookie => $"{cookie.Key}={cookie.Value}");
                client.DefaultRequestHeaders.Add("Cookie", cookieValues);

                var config = provider.GetRequiredService<IConfiguration>();
                var authHost = config.GetBootstrapAdminAuthenticationOptions().AuthHost.TrimEnd(new char[] { '/' });
                var url = $"{authHost}/api/Traces";
                client.BaseAddress = new Uri(url);
            });

            services.AddHttpClient<AppVeyorHttpClient>((provider, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            return services;
        }
    }
}
