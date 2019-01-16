using System.Collections.Generic;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class CategoryTest : ApiTest
    {
        public CategoryTest(BAWebHost factory) : base(factory, "Category", false)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            var cates = await Client.GetAsJsonAsync<IEnumerable<string>>("");
            Assert.NotEmpty(cates);
        }
    }
}
