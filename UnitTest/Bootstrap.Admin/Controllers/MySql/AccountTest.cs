using Xunit;

namespace Bootstrap.Admin.Controllers.MySql
{
    [Collection("MySqlContext")]
    public class AccountTest : Controllers.AccountTest
    {
        public AccountTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
