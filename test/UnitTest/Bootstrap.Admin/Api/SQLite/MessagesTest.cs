using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class MessagesTest : SqlServer.MessagesTest
    {
        public MessagesTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
