using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class RolesTest : SqlServer.RolesTest
    {
        public RolesTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
