using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class OnlineTest : SqlServer.OnlineTest
    {
        public OnlineTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
