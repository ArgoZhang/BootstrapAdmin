using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
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
