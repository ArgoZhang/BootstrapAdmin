using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class TracesTest : Api.TracesTest
    {
        public TracesTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
