using Longbow.Data;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [CollectionDefinition("MySqlContext")]
    public class MySqlContext : ICollectionFixture<BAMySqlWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BAMySqlWebHost : BALoginWebHost
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureDatabase(DatabaseProviderType.MySql);
        }
    }
}
