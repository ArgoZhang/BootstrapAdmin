using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class AnalyseTest : SqlServer.AnalyseTest
    {
        public AnalyseTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
