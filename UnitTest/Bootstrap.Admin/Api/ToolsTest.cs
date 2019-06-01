using System.Net;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class ToolsTest : ControllerTest
    {
        public ToolsTest(BAWebHost factory) : base(factory, "Tools/Index?ReturnUrl=http://localhost") { }

        [Fact]
        public async void Index_Ok()
        {
            var req = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, ""));
            Assert.Equal(HttpStatusCode.OK, req.StatusCode);
        }
    }
}
