using Bootstrap.DataAccess;
using System;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class ProfilesTest : ControllerTest
    {
        public ProfilesTest(BAWebHost factory) : base(factory, "api/Profiles") { }

        [Fact]
        public async void Put_Ok()
        {
            var usr = new User { UserName = "UnitTest_Change", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg", Css = "blue.css" };
            usr.Delete(usr.Retrieves().Where(u => u.UserName == usr.UserName).Select(u => u.Id));
            Assert.True(usr.Save(usr));

            // change theme
            usr.UserStatus = UserStates.ChangeTheme;
            var resp = await Client.PutAsJsonAsync<User, bool>(usr);
            Assert.True(resp);

            // Login as new user
            var client = Host.CreateClient();
            await client.LoginAsync("UnitTest_Change", "1");

            // change password
            usr.UserStatus = UserStates.ChangePassword;
            usr.NewPassword = "1";
            usr.Password = "1";
            resp = await client.PutAsJsonAsync<User, bool>("/api/Profiles", usr);
            Assert.True(resp);

            // change displayname
            usr.UserStatus = UserStates.ChangeDisplayName;
            resp = await client.PutAsJsonAsync<User, bool>("/api/Profiles", usr);
            Assert.True(resp);

            // change app
            usr.App = "UnitTest";
            usr.UserStatus = UserStates.SaveApp;
            resp = await client.PutAsJsonAsync<User, bool>("/api/Profiles", usr);
            Assert.True(resp);

            // delete 
            usr.Delete(usr.Retrieves().Where(u => u.UserName == usr.UserName).Select(u => u.Id));
        }
    }
}
