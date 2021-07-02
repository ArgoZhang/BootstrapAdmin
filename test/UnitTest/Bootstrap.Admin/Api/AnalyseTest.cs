using System.Net.Http;
using System.Net.Http.Json;
using Xunit;
using static Bootstrap.Admin.Controllers.Api.AnalyseController;

namespace Bootstrap.Admin.Api
{
    public class AnalyseTest : ControllerTest
    {
        public AnalyseTest(BALoginWebHost factory) : base(factory, "api/Analyse") { }

        [Fact]
        public async void Get_Ok()
        {
            var cates = await Client.GetFromJsonAsync<AnalyseData>("?logType=LoginUsers");
            Assert.NotNull(cates);
            cates = await Client.GetFromJsonAsync<AnalyseData>("?logType=log");
            Assert.NotNull(cates);
            cates = await Client.GetFromJsonAsync<AnalyseData>("?logType=trace");
            Assert.NotNull(cates);
        }
    }
}
