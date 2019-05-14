using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class DictTest : SqlServer.DictTest
    {
        public DictTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
