using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class SQLTest : SqlServer.SQLTest
    {
        public SQLTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
