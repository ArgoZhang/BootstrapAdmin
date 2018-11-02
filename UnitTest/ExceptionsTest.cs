using Bootstrap.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class ExceptionsTest
    {
        [Fact]
        public void RetrieveExceptions_Array()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("DB:0:Enabled", "true"),
                    new KeyValuePair<string, string>("DB:0:Widget", "Bootstrap.DataAccess"),
                    new KeyValuePair<string, string>("DB:0:ConnectionStrings:ba", "Data Source=.;Initial Catalog=BootstrapAdmin;User ID=sa;Password=sa")
                })
              .Build();

            new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddConfigurationManager(config)
                .AddDbAdapter(config);

            Exceptions excep = new Exceptions();
            var result = excep.RetrieveExceptions();
            var num = result.Count();

            Assert.True(num >= 0);
        }

        [Fact]
        public void RetrieveExceptions_SQLite_Array()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("DB:0:Enabled", "true"),
                    new KeyValuePair<string, string>("DB:0:Widget", "Bootstrap.DataAccess.SQLite"),
                    new KeyValuePair<string, string>("DB:0:DBProviderFactory", "Microsoft.Data.Sqlite.SqliteFactory, Microsoft.Data.Sqlite"),
                    new KeyValuePair<string, string>("DB:0:ConnectionStrings:ba", "Data Source=BootstrapAdmin.db;")
             })
              .Build();

            new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddConfigurationManager(config)
                .AddDbAdapter(config);

            Exceptions excep = new Exceptions();
            var result = excep.RetrieveExceptions();
            var num = result.Count();

            Assert.True(num >= 0);
        }

        [Fact]
        public void RetrieveExceptions_MySQL_Array()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("DB:0:Enabled", "true"),
                    new KeyValuePair<string, string>("DB:0:Widget", "Bootstrap.DataAccess.MySQL"),
                    new KeyValuePair<string, string>("DB:0:DBProviderFactory", "MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data"),
                    new KeyValuePair<string, string>("DB:0:ConnectionStrings:ba", "Server=10.211.55.2;Database=BA;Uid=argozhang;Pwd=argo@163.com;SslMode=none;")
                })
              .Build();

            new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddConfigurationManager(config)
                .AddDbAdapter(config);

            Exceptions excep = new Exceptions();
            var result = excep.RetrieveExceptions();
            var num = result.Count();

            Assert.True(num >= 0);
        }

        [Fact]
        public async void ClearExceptions_Void()
        {
            StaticMethods.Setup();
            Exceptions excep = new Exceptions();

            var method = excep.GetType().GetMethod("ClearExceptions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            method.Invoke(excep, null);
            await System.Threading.Tasks.Task.Delay(1000);
        }
    }
}
