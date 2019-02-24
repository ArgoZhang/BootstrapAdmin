using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class AppsTest : Api.AppsTest
    {
        public AppsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
