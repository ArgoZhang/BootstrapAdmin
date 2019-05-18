using Longbow.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using UnitTest;
using Xunit;

namespace Bootstrap.Admin
{
    [CollectionDefinition("SQLServerContext")]
    public class BootstrapAdminTestContext : ICollectionFixture<BAWebHost>
    {

    }

    [CollectionDefinition("SQLiteContext")]
    public class SQLiteContext : ICollectionFixture<SQLiteBAWebHost>
    {

    }

    [CollectionDefinition("MySqlContext")]
    public class MySqlContext : ICollectionFixture<MySqlBAWebHost>
    {

    }

    [CollectionDefinition("MongoContext")]
    public class MongoContext : ICollectionFixture<MongoBAWebHost>
    {

    }

    public class MySqlBAWebHost : BAWebHost
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            TestHelper.ConfigureWebHost(builder, DatabaseProviderType.MySql);
        }
    }

    public class SQLiteBAWebHost : BAWebHost
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            TestHelper.ConfigureWebHost(builder, DatabaseProviderType.SQLite);
        }
    }

    public class MongoBAWebHost : BAWebHost
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                new KeyValuePair<string, string>("DB:1:Enabled", "false"),
                new KeyValuePair<string, string>("DB:2:Enabled", "false"),
                new KeyValuePair<string, string>("DB:3:Enabled", "false")
            }));
        }
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

            builder.ConfigureAppConfiguration(app => app.AddJsonFile(TestHelper.RetrievePath($"UnitTest{Path.DirectorySeparatorChar}appsettings.json"), false, true));
            if (con.GetValue("Appveyor", false))
            {
                builder.ConfigureAppConfiguration(app => app.AddJsonFile(TestHelper.RetrievePath($"UnitTest{Path.DirectorySeparatorChar}appsettings.appveyor.json"), false, true));
            }
            TestHelper.ConfigureWebHost(builder);
        }
    }
}
