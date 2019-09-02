using Xunit;

namespace Bootstrap.Admin.Controllers.SQLite
{
    [Collection("SQLiteContext")]
    public class AccountTest : SqlServer.AccountTest
    {
        public AccountTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
