using Xunit;

namespace Bootstrap.DataAccess.MongoDB
{
    [Collection("MongoContext")]
    public class DBLogTest : DataAccess.DBLogTest
    {
        [Fact]
        public override void Save_Ok()
        {
            Assert.True(new DBLog().Save(null));
        }
    }
}
