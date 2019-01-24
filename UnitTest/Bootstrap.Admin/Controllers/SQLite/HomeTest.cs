using Xunit;

namespace Bootstrap.Admin.Controllers.SQLite
{
    [Collection("SQLiteContext")]
    public class HomeTest : Controllers.HomeTest
    {
        public HomeTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
