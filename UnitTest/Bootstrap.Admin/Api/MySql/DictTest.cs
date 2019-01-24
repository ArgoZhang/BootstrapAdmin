using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class DictTest : Api.DictTest
    {
        public DictTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
