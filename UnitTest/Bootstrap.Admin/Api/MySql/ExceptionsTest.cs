using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class ExceptionsTest : Api.ExceptionsTest
    {
        public ExceptionsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
