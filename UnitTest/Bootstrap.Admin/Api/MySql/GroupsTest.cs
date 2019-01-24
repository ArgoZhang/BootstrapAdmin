using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class GroupsTest : Api.GroupsTest
    {
        public GroupsTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
