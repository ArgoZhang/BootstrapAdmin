using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class InterfaceTest : SqlServer.InterfaceTest
    {
        public InterfaceTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
