using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class RolesTest : Api.RolesTest
    {
        public RolesTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
