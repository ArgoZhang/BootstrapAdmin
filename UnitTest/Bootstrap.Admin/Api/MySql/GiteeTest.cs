using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class GiteeTest : SqlServer.GiteeTest
    {
        public GiteeTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
