using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class LoginTest : Api.LoginTest
    {
        public LoginTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
