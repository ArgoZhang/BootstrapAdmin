using Bootstrap.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class InterfaceTest : ControllerTest
    {
        public InterfaceTest(BALoginWebHost factory) : base(factory, "api/Interface") { }

        [Fact]
        public async void RetrieveDicts_Ok()
        {
            var req = await Client.PostAsJsonAsync<string>("RetrieveDicts", "");
            var ret = await req.Content.ReadFromJsonAsync<IEnumerable<BootstrapDict>>();
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void RetrieveRolesByUrl_Ok()
        {
            var req = await Client.PostAsJsonAsync<string>("RetrieveRolesByUrl", "~/Admin/Index");
            var ret = await req.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void RetrieveRolesByUserName_Ok()
        {
            var req = await Client.PostAsJsonAsync<string>("RetrieveRolesByUserName", "Admin");
            var ret = await req.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void RetrieveUserByUserName_Ok()
        {
            var req = await Client.PostAsJsonAsync<string>("RetrieveUserByUserName", "Admin");
            var ret = await req.Content.ReadFromJsonAsync<BootstrapUser>();
            Assert.Equal("Admin", ret.UserName);
        }

        [Fact]
        public async void RetrieveAppMenus_Ok()
        {
            var req = await Client.PostAsJsonAsync<AppMenuOption>("RetrieveAppMenus", new AppMenuOption() { AppId = "Demo", UserName = "Admin", Url = "~/Admin/Index" });
            var ret = await req.Content.ReadFromJsonAsync<IEnumerable<BootstrapMenu>>();
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Healths_Ok()
        {
            var req = await Client.PostAsJsonAsync<string>("Healths", "UnitTest");
            Assert.False(req.IsSuccessStatusCode);
        }
    }
}
