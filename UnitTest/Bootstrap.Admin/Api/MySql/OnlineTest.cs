using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class OnlineTest : Api.OnlineTest
    {
        public OnlineTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
