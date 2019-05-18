using Xunit;

namespace Bootstrap.DataAccess.MongoDB
{
    [Collection("MongoContext")]
    public class DictsTest : SqlServer.DictsTest
    {
        protected override string DatabaseName { get; set; } = "MongoDB";
    }
}
