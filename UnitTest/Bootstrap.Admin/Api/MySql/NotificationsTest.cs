using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class NotificationsTest : Api.NotificationsTest
    {
        public NotificationsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
