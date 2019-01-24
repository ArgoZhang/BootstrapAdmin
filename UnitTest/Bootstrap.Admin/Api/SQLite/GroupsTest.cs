using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class GroupsTest : Api.GroupsTest
    {
        public GroupsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
