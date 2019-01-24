using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class CategoryTest : Api.CategoryTest
    {
        public CategoryTest(MySqlBAWebHost factory) : base(factory) { }
    }
}
