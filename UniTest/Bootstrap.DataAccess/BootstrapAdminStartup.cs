using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    public class BootstrapAdminStartup
    {
        public BootstrapAdminStartup()
        {
            var sqlConnectionStrings = "Data Source=.;Initial Catalog=UnitTest;User ID=sa;Password=sa";
            var mysqlConnectionStrings = "Server=10.211.55.2;Database=BA;Uid=argozhang;Pwd=argo@163.com;SslMode=none;";
            var config = new ConfigurationBuilder().AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("ConnectionStrings:ba", sqlConnectionStrings),
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),

                new KeyValuePair<string, string>("DB:1:Enabled", "false"),
                new KeyValuePair<string, string>("DB:1:ProviderName", "SQLite"),
                new KeyValuePair<string, string>("DB:1:ConnectionStrings:ba", "Data Source=UnitTest.db"),

                new KeyValuePair<string, string>("DB:2:Enabled", "false"),
                new KeyValuePair<string, string>("DB:2:ProviderName", "MySql"),
                new KeyValuePair<string, string>("DB:2:ConnectionStrings:ba", mysqlConnectionStrings),
                new KeyValuePair<string, string>("LongbowCache:Enabled", "false")
            }).Build();
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(config);
            sc.AddConfigurationManager(config);
            sc.AddCacheManager(config);
            sc.AddDbAdapter();
        }
    }
}
