using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class TracesTest : SqlServer.TracesTest
    {
        public TracesTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
