using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class RolesTest : SqlServer.RolesTest
    {
        public RolesTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
