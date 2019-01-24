using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class NotificationsTest : Api.NotificationsTest
    {
        public NotificationsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
