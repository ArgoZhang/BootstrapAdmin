using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class ProfilesTest : SqlServer.ProfilesTest
    {
        public ProfilesTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
