using Bootstrap.Security;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class InterfaceTest : ControllerTest
    {
        public InterfaceTest(BAWebHost factory) : base(factory, "api/Interface") { }

        [Fact]
        public async void RetrieveDicts_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<BootstrapDict>>("RetrieveDicts", "");
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void RetrieveRolesByUrl_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<string>>("RetrieveRolesByUrl", "~/Admin/Index");
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void RetrieveRolesByUserName_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<string>>("RetrieveRolesByUserName", "Admin");
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void RetrieveUserByUserName_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, BootstrapUser>("RetrieveUserByUserName", "Admin");
            Assert.Equal("Admin", ret.UserName);
        }

        [Fact]
        public async void RetrieveAppMenus_Ok()
        {
            var ret = await Client.PostAsJsonAsync<AppMenuOption, IEnumerable<BootstrapMenu>>("RetrieveAppMenus", new AppMenuOption() { AppId = "Demo", UserName = "Admin", Url = "~/Admin/Index" });
            Assert.NotEmpty(ret);
        }
    }
}
