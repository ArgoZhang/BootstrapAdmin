using Xunit;

namespace Bootstrap.Admin.Controllers.MySql
{
    [Collection("MySqlContext")]
    public class HomeTest : SqlServer.HomeTest
    {
        public HomeTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
