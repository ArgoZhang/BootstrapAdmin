using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class ExceptionsTest : Api.ExceptionsTest
    {
        public ExceptionsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
