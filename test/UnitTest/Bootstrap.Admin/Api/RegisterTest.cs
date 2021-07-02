using Bootstrap.DataAccess;
using System.Linq;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class RegisterTest : ControllerTest
    {
        public RegisterTest(BALoginWebHost factory) : base(factory, "api/Register") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetFromJsonAsync<bool>("?userName=Admin");
            Assert.False(resp);
            resp = await Client.GetFromJsonAsync<bool>("?userName=Admin1");
            Assert.True(resp);
        }

        [Fact]
        public async void Post_Ok()
        {
            // register new user
            var nusr = new User() { UserName = "U_Register", DisplayName = "UnitTest", Password = "1", Description = "UnitTest" };
            var resp = await Client.PostAsJsonAsync<User>("", nusr);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            ret = await Client.GetFromJsonAsync<bool>($"?userName={nusr.UserName}");
            Assert.False(ret);
            UserHelper.Delete(nusr.RetrieveNewUsers().Where(u => u.UserName == nusr.UserName).Select(u => u.Id));
        }

        [Fact]
        public async void Put_Ok()
        {
            var user = new ResetUser() { DisplayName = "UnitTest", UserName = "UnitTest", Reason = "UnitTest" };
            var resp = await Client.PutAsJsonAsync<ResetUser>("", user);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }

        [Fact]
        public async void Put_UserName()
        {
            var user = new User() { Password = "1" };
            var resp = await Client.PutAsJsonAsync<User>("UnitTest", user);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.False(ret);

            // 重置Admin密码
            await Client.PutAsJsonAsync<ResetUser>("", new ResetUser { UserName = "Admin", DisplayName = "Administrator", Reason = "UnitTest" });
            resp = await Client.PutAsJsonAsync<User>("Admin", new User() { Password = "123789" });
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }
    }
}
