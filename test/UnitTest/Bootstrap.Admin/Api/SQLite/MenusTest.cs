using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class MenusTest : SqlServer.MenusTest
    {
        public MenusTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
