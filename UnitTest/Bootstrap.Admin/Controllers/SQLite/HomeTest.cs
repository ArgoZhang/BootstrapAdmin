using Xunit;

namespace Bootstrap.Admin.Controllers.SQLite
{
    [Collection("SQLiteContext")]
    public class HomeTest : SqlServer.HomeTest
    {
        public HomeTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
