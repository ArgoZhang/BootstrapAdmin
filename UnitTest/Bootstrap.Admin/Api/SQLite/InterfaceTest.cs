using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class InterfaceTest : Api.InterfaceTest
    {
        public InterfaceTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
