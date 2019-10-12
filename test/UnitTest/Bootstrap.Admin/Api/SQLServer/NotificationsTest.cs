using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class NotificationsTest : ControllerTest
    {
        public NotificationsTest(BAWebHost factory) : base(factory, "api/Notifications") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<object>();
            Assert.NotNull(resp);
        }
    }
}
