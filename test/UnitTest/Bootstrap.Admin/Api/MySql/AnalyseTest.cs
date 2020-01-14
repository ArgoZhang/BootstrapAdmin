using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class AnalyseTest : SqlServer.AnalyseTest
    {
        public AnalyseTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
