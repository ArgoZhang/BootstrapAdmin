using Xunit;

namespace Bootstrap.Admin.Controllers.MySql
{
    [Collection("MySqlContext")]
    public class AccountTest : SqlServer.AccountTest
    {
        public AccountTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
