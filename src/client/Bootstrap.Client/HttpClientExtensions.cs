using Bootstrap.Client.DataAccess;
using Longbow.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    internal static class HttpClientExtensions
    {
        public static IServiceCollection AddBootstrapHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("BootstrapAdmin", client => client.DefaultRequestHeaders.Connection.Add("keep-alive"));
            services.AddHttpClient<TraceHttpClient>((provider, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Connection.Add("keep-alive");

                // set auth
                var context = provider.GetRequiredService<IHttpContextAccessor>();
                var cookieValues = context.HttpContext.Request.Cookies.Select(cookie => $"{cookie.Key}={cookie.Value}");
                client.DefaultRequestHeaders.Add("Cookie", cookieValues);

                var authHost = ConfigurationManager.Get<BootstrapAdminAuthenticationOptions>().AuthHost.TrimEnd(new char[] { '/' });
                var url = $"{authHost}/api/Traces";
                client.BaseAddress = new Uri(url);
            });
            return services;
        }
    }
}
