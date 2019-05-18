using Xunit;

namespace Bootstrap.DataAccess.SQLite
{
    [Collection("SQLiteContext")]
    public class DictsTest : SqlServer.DictsTest
    {
        protected override string DatabaseName { get; set; } = "SQLite";
    }
}
