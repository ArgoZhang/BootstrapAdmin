using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class MessagesTest : SqlServer.MessagesTest
    {
        public MessagesTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
