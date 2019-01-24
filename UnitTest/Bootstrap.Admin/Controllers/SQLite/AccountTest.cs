using Xunit;

namespace Bootstrap.Admin.Controllers.SQLite
{
    [Collection("SQLiteContext")]
    public class AccountTest : Controllers.AccountTest
    {
        public AccountTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
