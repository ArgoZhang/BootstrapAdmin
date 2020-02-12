using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class NotificationsTest : ControllerTest
    {
        public NotificationsTest(BALoginWebHost factory) : base(factory, "api/Notifications") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<object>();
            Assert.NotNull(resp);
        }
    }
}
