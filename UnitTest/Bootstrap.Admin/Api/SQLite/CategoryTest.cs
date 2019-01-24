using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class CategoryTest : Api.CategoryTest
    {
        public CategoryTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}
