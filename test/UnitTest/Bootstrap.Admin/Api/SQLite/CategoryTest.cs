using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class CategoryTest : SqlServer.CategoryTest
    {
        public CategoryTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
