using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class MessagesTest : Api.MessagesTest
    {
        public MessagesTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
