using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class NewTest : SqlServer.NewTest
    {
        public NewTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
