using Xunit;

namespace Bootstrap.DataAccess.MongoDB
{
    [Collection("MongoContext")]
    public class UsersTest : DataAccess.UsersTest
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
