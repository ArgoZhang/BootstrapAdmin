using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("BootstrapAdminTestContext")]
    public class MessagesTest
    {
        [Fact]
        public void RetrieveHeaders_Ok()
        {
            var m = new Message();
            m.RetrieveHeaders("Admin");
        }
    }
}
