using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class SettingsTest : SqlServer.SettingsTest
    {
        public SettingsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
