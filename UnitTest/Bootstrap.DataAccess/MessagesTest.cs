using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class MessagesTest
    {
        [Fact]
        public void RetrieveHeaders_Ok()
        {
            Assert.NotNull(MessageHelper.Retrieves("Admin"));
        }
    }
}
