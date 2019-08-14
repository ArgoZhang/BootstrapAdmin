using Xunit;

namespace Bootstrap.Admin.Api
{
    public class HealthCheckTest : ControllerTest
    {
        public HealthCheckTest(BAWebHost factory) : base(factory, "") { }

        [Fact]
        public async void Get_Ok()
        {
            var cates = await Client.GetAsJsonAsync<object>("/Healths");
            Assert.NotNull(cates);
        }

        [Fact]
        public async void UI_Ok()
        {
            var cates = await Client.GetStringAsync("/Healths-ui");
            Assert.Contains("健康检查", cates);
        }
    }
}
