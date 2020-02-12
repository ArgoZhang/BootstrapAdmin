using Xunit;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 正常系统
    /// </summary>
    [CollectionDefinition("Login")]
    public class BootstrapAdminContext : ICollectionFixture<BALoginWebHost>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class BALoginWebHost : BAWebHost
    {
        /// <summary>
        /// 
        /// </summary>
        public BALoginWebHost()
        {
            var client = CreateClient("Account/Login");
            var login = client.LoginAsync();
            login.Wait();
        }
    }
}
