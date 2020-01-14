using Xunit;

namespace Bootstrap.DataAccess.MongoDB
{
    [Collection("MongoContext")]
    public class UsersTest : SqlServer.UsersTest
    {
        [Fact]
        public void RejectUser_Ok()
        {
            var user = new RejectUser()
            {
                Id = ""
            };
        }
    }
}
