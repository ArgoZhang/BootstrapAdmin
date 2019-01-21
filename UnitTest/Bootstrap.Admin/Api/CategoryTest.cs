using System.Collections.Generic;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class CategoryTest : ControllerTest
    {
        public CategoryTest(BAWebHost factory) : base(factory, "api/Category") { }

        [Fact]
        public async void Get_Ok()
        {
            var cates = await Client.GetAsJsonAsync<IEnumerable<string>>();
            Assert.NotEmpty(cates);
        }
    }
}
