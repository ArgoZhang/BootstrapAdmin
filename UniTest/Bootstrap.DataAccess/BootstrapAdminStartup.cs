using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    public class BootstrapAdminStartup
    {
        public BootstrapAdminStartup()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("ConnectionStrings:ba", "Data Source=.;Initial Catalog=UnitTest;User ID=sa;Password=sa"),
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),
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
