using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class RegisterTest : SqlServer.RegisterTest
    {
        public RegisterTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
