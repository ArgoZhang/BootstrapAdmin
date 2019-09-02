using Xunit;

namespace Bootstrap.Admin.Controllers.SQLite
{
    [Collection("SQLiteContext")]
    public class AdminTest : SqlServer.AdminTest
    {
        public AdminTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
