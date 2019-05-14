using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class LogsTest : SqlServer.LogsTest
    {
        public LogsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
