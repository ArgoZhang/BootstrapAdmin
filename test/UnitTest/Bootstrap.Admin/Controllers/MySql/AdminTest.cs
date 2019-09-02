using Xunit;

namespace Bootstrap.Admin.Controllers.MySql
{
    [Collection("MySqlContext")]
    public class AdminTest : SqlServer.AdminTest
    {
        public AdminTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
