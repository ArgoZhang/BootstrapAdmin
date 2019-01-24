using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class DictTest : Api.DictTest
    {
        public DictTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
