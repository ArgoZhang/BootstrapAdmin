using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class OnlineTest : SqlServer.OnlineTest
    {
        public OnlineTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
