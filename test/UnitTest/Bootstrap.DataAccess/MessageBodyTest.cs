using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class MessageBodyTest
    {
        [Fact]
        public void ToString_Ok()
        {
            var body = new MessageBody();
            body.Category = "UnitTest";
            body.Message = "UnitTest";
            Assert.Equal($"{body.Category}-{body.Message}", body.ToString());
        }
    }
}
