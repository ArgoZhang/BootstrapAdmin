using Xunit;
using static Bootstrap.Admin.Controllers.Api.AnalyseController;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class AnalyseTest : ControllerTest
    {
        public AnalyseTest(BAWebHost factory) : base(factory, "api/Analyse") { }

        [Fact]
        public async void Get_Ok()
        {
            var cates = await Client.GetAsJsonAsync<AnalyseData>("?logType=LoginUsers");
            Assert.NotNull(cates);
            cates = await Client.GetAsJsonAsync<AnalyseData>("?logType=log");
            Assert.NotNull(cates);
            cates = await Client.GetAsJsonAsync<AnalyseData>("?logType=trace");
            Assert.NotNull(cates);

        }
    }
}
