using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class RegisterTest : Api.RegisterTest
    {
        public RegisterTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
