using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class CategoryTest : SqlServer.CategoryTest
    {
        public CategoryTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
