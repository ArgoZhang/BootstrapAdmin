using Bootstrap.DataAccess;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class RegisterTest : ApiWebHost
    {
        public RegisterTest(BAWebHost factory) : base(factory, "Register", true)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<bool>("?userName=Admin");
            Assert.False(resp);
        }

        [Fact]
        public async void Post_Ok()
        {
            // register new user
            var nusr = new User() { UserName = "UnitTest-RegisterController", DisplayName = "UnitTest", Password = "1", Description = "UnitTest" };
            var resp = await Client.PostAsJsonAsync<User, bool>("", nusr);
            Assert.True(resp);

            nusr.Delete(nusr.RetrieveNewUsers().Where(u => u.UserName == nusr.UserName).Select(u => u.Id));
        }
    }
}
