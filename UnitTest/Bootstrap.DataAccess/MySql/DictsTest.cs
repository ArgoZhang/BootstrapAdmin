using Xunit;

namespace Bootstrap.DataAccess.MySql
{
    [Collection("MySqlContext")]
    public class DictsTest : SqlServer.DictsTest
    {
        protected override string DatabaseName { get; set; } = "MySql";
    }
}
