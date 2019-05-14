using Bootstrap.DataAccess;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class ProfilesTest : ControllerTest
    {
        public ProfilesTest(BAWebHost factory) : base(factory, "api/Profiles") { }

        [Fact]
        public async void Put_SaveTheme()
        {
            var usr = new User { UserName = "Admin" };
            // change theme
            usr.Css = "blue.css";
            usr.UserStatus = UserStates.ChangeTheme;
            var resp = await Client.PutAsJsonAsync<User, bool>(usr);
            Assert.True(resp);
        }

        [Fact]
        public async void Put_ChangePassword()
        {
            var usr = new User() { UserName = "Admin" };
            // change password
            usr.UserStatus = UserStates.ChangePassword;
            usr.NewPassword = "123789";
            usr.Password = "123789";
            var resp = await Client.PutAsJsonAsync<User, bool>(usr);
            Assert.True(resp);
        }

        [Fact]
        public async void Put_ChangeDisplayName()
        {
            var usr = new User() { UserName = "Admin" };
            // change displayname
            usr.UserStatus = UserStates.ChangeDisplayName;
            usr.DisplayName = "Administrator";
            var resp = await Client.PutAsJsonAsync<User, bool>(usr);
            Assert.True(resp);
        }

        [Fact]
        public async void Put_SaveApp()
        {
            var usr = new User() { UserName = "Admin" };
            // change app
            usr.App = "UnitTest";
            usr.UserStatus = UserStates.SaveApp;
            var resp = await Client.PutAsJsonAsync<User, bool>(usr);
            Assert.True(resp);
        }
    }
}
