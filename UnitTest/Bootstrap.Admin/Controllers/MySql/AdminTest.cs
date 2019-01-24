using Xunit;

namespace Bootstrap.Admin.Controllers.MySql
{
    [Collection("MySqlContext")]
    public class AdminTest : Controllers.AdminTest
    {
        public AdminTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
