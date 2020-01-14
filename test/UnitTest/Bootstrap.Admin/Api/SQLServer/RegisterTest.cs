using Bootstrap.DataAccess;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class RegisterTest : ControllerTest
    {
        public RegisterTest(BAWebHost factory) : base(factory, "api/Register") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<bool>("?userName=Admin");
            Assert.False(resp);
            resp = await Client.GetAsJsonAsync<bool>("?userName=Admin1");
            Assert.True(resp);
        }

        [Fact]
        public async void Post_Ok()
        {
            // register new user
            var nusr = new User() { UserName = "U_Register", DisplayName = "UnitTest", Password = "1", Description = "UnitTest" };
            var resp = await Client.PostAsJsonAsync<User, bool>("", nusr);
            Assert.True(resp);
            resp = await Client.GetAsJsonAsync<bool>($"?userName={nusr.UserName}");
            Assert.False(resp);
            UserHelper.Delete(nusr.RetrieveNewUsers().Where(u => u.UserName == nusr.UserName).Select(u => u.Id));
        }

        [Fact]
        public async void Put_Ok()
        {
            var user = new ResetUser() { DisplayName = "UnitTest", UserName = "UnitTest", Reason = "UnitTest" };
            var resp = await Client.PutAsJsonAsync<ResetUser, bool>("", user);
            Assert.True(resp);
        }

        [Fact]
        public async void Put_UserName()
        {
            var user = new User() { Password = "1" };
            var resp = await Client.PutAsJsonAsync<User, bool>("UnitTest", user);
            Assert.False(resp);

            // 重置Admin密码
            await Client.PutAsJsonAsync<ResetUser, bool>("", new ResetUser { UserName = "Admin", DisplayName = "Administrator", Reason = "UnitTest" });
            resp = await Client.PutAsJsonAsync<User, bool>("Admin", new User() { Password = "123789" });
            Assert.True(resp);
        }
    }
}
