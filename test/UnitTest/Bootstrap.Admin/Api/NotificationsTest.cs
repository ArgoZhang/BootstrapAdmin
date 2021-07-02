using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class NotificationsTest : ControllerTest
    {
        public NotificationsTest(BALoginWebHost factory) : base(factory, "api/Notifications") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetFromJsonAsync<object>("");
            Assert.NotNull(resp);
        }
    }
}
