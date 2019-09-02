using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class RegisterTest : SqlServer.RegisterTest
    {
        public RegisterTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
