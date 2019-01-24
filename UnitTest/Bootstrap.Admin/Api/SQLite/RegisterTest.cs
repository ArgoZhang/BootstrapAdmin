using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class RegisterTest : Api.RegisterTest
    {
        public RegisterTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
