using Xunit;

namespace Bootstrap.Admin.Controllers.MySql
{
    [Collection("MySqlContext")]
    public class HomeTest : Controllers.HomeTest
    {
        public HomeTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
