using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class MenusTest : Api.MenusTest
    {
        public MenusTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
