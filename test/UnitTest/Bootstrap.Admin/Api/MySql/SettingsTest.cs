using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class SettingsTest : SqlServer.SettingsTest
    {
        public SettingsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
