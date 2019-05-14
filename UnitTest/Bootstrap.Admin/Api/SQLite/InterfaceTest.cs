using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class InterfaceTest : SqlServer.InterfaceTest
    {
        public InterfaceTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
