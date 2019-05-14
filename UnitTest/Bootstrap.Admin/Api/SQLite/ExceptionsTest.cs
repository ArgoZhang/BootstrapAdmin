using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class ExceptionsTest : SqlServer.ExceptionsTest
    {
        public ExceptionsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
