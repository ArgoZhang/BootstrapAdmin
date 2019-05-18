using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class GiteeTest : ControllerTest
    {
        public GiteeTest(BAWebHost factory) : base(factory, "api/Gitee") { }

        [Fact]
        public async void Issues_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Issues");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Pulls_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Pulls");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Releases_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Releases");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void Builds_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("Builds");
            Assert.NotNull(cates);
        }
    }
}
