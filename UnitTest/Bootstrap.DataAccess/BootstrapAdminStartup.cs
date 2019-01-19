using Microsoft.Extensions.DependencyInjection;
using UnitTest;

namespace Bootstrap.DataAccess
{
    public class BootstrapAdminStartup
    {
        static BootstrapAdminStartup()
        {
            var config = TestHelper.CreateConfiguraton();

            var sc = new ServiceCollection();
            sc.AddSingleton(config);
            sc.AddConfigurationManager(config);
            sc.AddCacheManager(config);
            sc.AddDbAdapter();
        }
    }
}
