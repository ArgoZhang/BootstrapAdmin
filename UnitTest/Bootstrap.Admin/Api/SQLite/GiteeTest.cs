using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class GiteeTest : SqlServer.GiteeTest
    {
        public GiteeTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
