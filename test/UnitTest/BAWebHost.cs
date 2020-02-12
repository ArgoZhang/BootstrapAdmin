using Longbow.Web.SMS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 未登录
    /// </summary>
    [CollectionDefinition("BA-Logout")]
    public class BootstrapAdminLogoutContext : ICollectionFixture<BAWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BAWebHost : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// 获得已经登录的HttpClient
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public HttpClient CreateClient(string baseAddress)
        {
            var client = CreateDefaultClient(new Uri($"http://localhost/{baseAddress}/"), new RedirectHandler(7), new CookieContainerHandler(_cookie));
            return client;
        }

        protected override void ConfigureClient(HttpClient client)
        {
            base.ConfigureClient(client);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_1_UnitTest) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0.1 Safari/605.1.15");
        }

        private readonly CookieContainer _cookie = new CookieContainer();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var config = new ConfigurationBuilder();
            config.AddEnvironmentVariables();
            var con = config.Build();

            // 增加单元测试本身的配置文件
            builder.ConfigureAppConfiguration(app => app.AddJsonFile(TestHelper.RetrievePath($"UnitTest{Path.DirectorySeparatorChar}appsettings.json"), false, true));
            if (con.GetValue("Appveyor", false))
            {
                builder.ConfigureAppConfiguration(app => app.AddJsonFile(TestHelper.RetrievePath($"UnitTest{Path.DirectorySeparatorChar}appsettings.appveyor.json"), false, true));
            }

            // 替换 SMS 服务
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ISMSProvider, DefaultSMSProvider>();
            });
        }
    }
}
