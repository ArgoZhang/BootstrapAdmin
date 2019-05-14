using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class ExceptionsTest : SqlServer.ExceptionsTest
    {
        public ExceptionsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
