using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class LoginTest : SqlServer.LoginTest
    {
        public LoginTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
