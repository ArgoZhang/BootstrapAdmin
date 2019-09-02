using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class AppsTest : SqlServer.AppsTest
    {
        public AppsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
