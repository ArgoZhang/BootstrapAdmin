using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class NewTest : ControllerTest
    {
        public NewTest(BAWebHost factory) : base(factory, "api/New") { }

        [Fact]
        public async void Get_Ok()
        {
            var nusr = InsertNewUser();

            var resp = await Client.GetAsJsonAsync<IEnumerable<object>>();
            Assert.NotEmpty(resp);

            // 删除新用户
            DeleteUnitTestUser();
        }

        [Fact]
        public async void Put_Ok()
        {
            DeleteUnitTestUser();
            var nusr = InsertNewUser();

            // Approve
            nusr.UserStatus = UserStates.ApproveUser;
            var resp = await Client.PutAsJsonAsync<User, bool>(nusr);
            Assert.True(resp);

            // 删除新用户
            nusr.Delete(new string[] { nusr.Id });

            // Reject
            nusr = InsertNewUser();
            nusr.UserStatus = UserStates.RejectUser;
            resp = await Client.PutAsJsonAsync<User, bool>(nusr);
            Assert.True(resp);

            // 删除新用户
            DeleteUnitTestUser();
        }

        private User InsertNewUser()
        {
            // 插入新用户
            var nusr = new User() { UserName = "UnitTest-Register", DisplayName = "UnitTest", Password = "1", Description = "UnitTest" };
            Assert.True(new User().Save(nusr));
            return nusr;
        }

        private void DeleteUnitTestUser()
        {
            var ids = new User().RetrieveNewUsers().Where(u => u.UserName == "UnitTest-Register").Select(u => u.Id);
            new User().Delete(ids);
        }
    }
}
