using Xunit;

namespace Bootstrap.DataAccess
{
    public class MessagesTest : IClassFixture<BootstrapAdminStartup>
    {
        [Fact]
        public void RetrieveHeaders_Ok()
        {
            var m = new Message();
            m.RetrieveHeaders("Admin");
        }
    }
}
