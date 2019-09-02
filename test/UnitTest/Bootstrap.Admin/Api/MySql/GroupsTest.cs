using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class GroupsTest : SqlServer.GroupsTest
    {
        public GroupsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
