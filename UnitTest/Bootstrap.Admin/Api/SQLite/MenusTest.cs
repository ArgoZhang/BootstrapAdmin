using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class MenusTest : Api.MenusTest
    {
        public MenusTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
