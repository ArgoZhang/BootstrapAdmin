using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using System;
using System.Net;
using System.Net.Http;
using UnitTest;
using Xunit;

namespace Bootstrap.Admin
{
    [CollectionDefinition("BootstrapAdminTestContext")]
    public class BootstrapAdminTestContext : ICollectionFixture<BAWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BAWebHost : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// 
        /// </summary>
        static BAWebHost()
        {
            // Copy license
            TestHelper.CopyLicense();
        }

        public BAWebHost()
        {
            var client = CreateClient("Account/Login");
            var login = client.LoginAsync();
            login.Wait();
        }

        /// <summary>
        /// 获得已经登录的HttpClient
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public HttpClient CreateClient(string baseAddress) => CreateDefaultClient(new Uri($"http://localhost/{baseAddress}/"), new RedirectHandler(7), new CookieContainerHandler(_cookie));

        private readonly CookieContainer _cookie = new CookieContainer();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            TestHelper.ConfigureWebHost(builder);
        }
    }
}
