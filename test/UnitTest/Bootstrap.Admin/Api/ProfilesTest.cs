using Bootstrap.DataAccess;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class ProfilesTest : ControllerTest
    {
        public ProfilesTest(BALoginWebHost factory) : base(factory, "api/Profiles") { }

        [Fact]
        public async void Put_SaveTheme()
        {
            var usr = new User { UserName = "Admin" };
            // change theme
            usr.Css = "blue.css";
            usr.UserStatus = UserStates.ChangeTheme;
            var resp = await Client.PutAsJsonAsync<User>("", usr);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }

        [Fact]
        public async void Put_ChangePassword()
        {
            var usr = new User() { UserName = "Admin" };
            // change password
            usr.UserStatus = UserStates.ChangePassword;
            usr.NewPassword = "123789";
            usr.Password = "123789";
            var resp = await Client.PutAsJsonAsync<User>("", usr);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }

        [Fact]
        public async void Put_ChangeDisplayName()
        {
            var usr = new User() { UserName = "Admin" };
            // change displayname
            usr.UserStatus = UserStates.ChangeDisplayName;
            usr.DisplayName = "Administrator";
            var resp = await Client.PutAsJsonAsync<User>("", usr);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }

        [Fact]
        public async void Put_SaveApp()
        {
            var usr = new User() { UserName = "Admin" };
            // change app
            usr.App = "UnitTest";
            usr.UserStatus = UserStates.SaveApp;
            var resp = await Client.PutAsJsonAsync<User>("", usr);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);
        }

        [Fact]
        public async void SaveAndDelIcon_Ok()
        {
            var iconFile = TestHelper.RetrievePath(string.Format("..{0}src{0}admin{0}Bootstrap.Admin{0}wwwroot{0}images{0}logo.jpg", Path.DirectorySeparatorChar));
            var adminFile = TestHelper.RetrievePath(string.Format("..{0}src{0}admin{0}Bootstrap.Admin{0}wwwroot{0}images{0}uploader{0}Admin.jpg", Path.DirectorySeparatorChar));
            var fi = new FileInfo(iconFile);
            var fileName = fi.Name;
            var fileContents = File.ReadAllBytes(fi.FullName);

            var loginContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(fileContents);
            byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
            loginContent.Add(byteArrayContent, "fileName", fileName);

            var req = await Client.PostAsync("", loginContent);
            Assert.Equal(HttpStatusCode.OK, req.StatusCode);
            Assert.True(File.Exists(adminFile));

            // delete file
            var delContent = new StringContent("key=Admin.jpg");
            delContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            await Client.PostAsync("del", delContent);
            await Client.PostAsync("Delete", delContent);
            Assert.False(File.Exists(adminFile));
        }
    }
}
