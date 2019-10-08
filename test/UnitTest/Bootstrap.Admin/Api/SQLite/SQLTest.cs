using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class SQLTest : SqlServer.SQLTest
    {
        public SQLTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
