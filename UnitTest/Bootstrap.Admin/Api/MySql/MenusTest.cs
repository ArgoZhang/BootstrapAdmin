using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class MenusTest : SqlServer.MenusTest
    {
        public MenusTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
