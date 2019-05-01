using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class TracesTest : Api.TracesTest
    {
        public TracesTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
