using Xunit;

namespace Bootstrap.Admin.Api
{
    public class NotificationsTest : ApiWebHost
    {
        public NotificationsTest(BAWebHost factory) : base(factory, "Notifications", true)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<object>();
            Assert.NotNull(resp);
        }
    }
}
