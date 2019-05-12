using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class AppsTest : Api.AppsTest
    {
        public AppsTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
