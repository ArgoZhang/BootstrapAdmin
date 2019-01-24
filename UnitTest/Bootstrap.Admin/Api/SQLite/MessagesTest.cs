using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class MessagesTest : Api.MessagesTest
    {
        public MessagesTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
