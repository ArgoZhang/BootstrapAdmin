using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class NewTest : SqlServer.NewTest
    {
        public NewTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
