using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class CategoryTest : ControllerTest
    {
        public CategoryTest(BAWebHost factory) : base(factory, "api/Category") { }

        [Fact]
        public async void DictCategorys_Ok()
        {
            var cates = await Client.GetAsJsonAsync<IEnumerable<string>>("RetrieveDictCategorys");
            Assert.NotEmpty(cates);
        }

        [Fact]
        public async void Menus_Ok()
        {
            var cates = await Client.GetAsJsonAsync<IEnumerable<string>>("RetrieveMenus");
            Assert.NotEmpty(cates);
        }

        [Fact]
        public async void ParentMenus_Ok()
        {
            var cates = await Client.GetAsJsonAsync<IEnumerable<string>>("RetrieveParentMenus");
            Assert.NotEmpty(cates);
        }
    }
}
