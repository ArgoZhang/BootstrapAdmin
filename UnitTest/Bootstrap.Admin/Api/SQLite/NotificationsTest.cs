using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class NotificationsTest : SqlServer.NotificationsTest
    {
        public NotificationsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
