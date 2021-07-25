using Bootstrap.Admin.Controllers.Api;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class GiteeTest : ControllerTest
    {
        public GiteeTest(BALoginWebHost factory) : base(factory, "api/Gitee") { }

        [Fact]
        public async void Issues_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Issues");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Pulls_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Pulls");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Releases_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Releases");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Builds_Ok()
        {
            var cates = await Client.GetFromJsonAsync<object>("Builds");
            Assert.NotNull(cates);
        }
    }
}
