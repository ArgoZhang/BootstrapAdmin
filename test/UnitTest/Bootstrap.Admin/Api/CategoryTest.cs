using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class CategoryTest : ControllerTest
    {
        public CategoryTest(BALoginWebHost factory) : base(factory, "api/Category") { }

        [Fact]
        public async void DictCategorys_Ok()
        {
            var cates = await Client.GetFromJsonAsync<IEnumerable<string>>("RetrieveDictCategorys");
            Assert.NotEmpty(cates);
        }

        [Fact]
        public async void Menus_Ok()
        {
            var cates = await Client.GetFromJsonAsync<IEnumerable<string>>("RetrieveMenus");
            Assert.NotEmpty(cates);
        }

        [Fact]
        public async void ParentMenus_Ok()
        {
            var cates = await Client.GetFromJsonAsync<IEnumerable<string>>("RetrieveParentMenus");
            Assert.NotEmpty(cates);
        }

        [Fact]
        public async void ValidateMenuBySubMenu_Ok()
        {
            var id = MenuHelper.RetrieveAllMenus("Admin").First(m => m.Name == "个人中心").Id;
            var cates = await Client.GetFromJsonAsync<bool>($"ValidateMenuBySubMenu/{id}");
            Assert.False(cates);

            id = MenuHelper.RetrieveAllMenus("Admin").First(m => m.Name == "后台管理").Id;
            cates = await Client.GetFromJsonAsync<bool>($"ValidateMenuBySubMenu/{id}");
            Assert.True(cates);
        }

        [Fact]
        public async void ValidateParentMenuById_Ok()
        {
            var id = MenuHelper.RetrieveAllMenus("Admin").First(m => m.Name == "个人中心").Id;
            var cates = await Client.GetFromJsonAsync<bool>($"ValidateParentMenuById/{id}");
            Assert.True(cates);

            var subId = MenuHelper.RetrieveAllMenus("Admin").First(m => m.ParentId == id).Id;
            cates = await Client.GetFromJsonAsync<bool>($"ValidateParentMenuById/{subId}");
            Assert.False(cates);
        }
    }
}
