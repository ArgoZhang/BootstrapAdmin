using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class NewTest : ControllerTest
    {
        public NewTest(BALoginWebHost factory) : base(factory, "api/New") { }

        [Fact]
        public async void Get_Ok()
        {
            InsertNewUser();

            var resp = await Client.GetFromJsonAsync<IEnumerable<object>>("");
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
            var resp = await Client.PutAsJsonAsync<User>("", nusr);
            var ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            // 删除新用户
            UserHelper.Delete(new string[] { nusr.Id });

            // Reject
            nusr = InsertNewUser();
            nusr.UserStatus = UserStates.RejectUser;
            resp = await Client.PutAsJsonAsync<User>("", nusr);
            ret = await resp.Content.ReadFromJsonAsync<bool>();
            Assert.True(ret);

            // 删除新用户
            DeleteUnitTestUser();
        }

        private static User InsertNewUser()
        {
            // 插入新用户
            var nusr = new User() { UserName = "UnitTest_New", DisplayName = "UnitTest", Password = "1", Description = "UnitTest" };
            Assert.True(UserHelper.Save(nusr));
            return nusr;
        }

        private static void DeleteUnitTestUser()
        {
            var ids = UserHelper.RetrieveNewUsers().Where(u => u.UserName == "UnitTest_New").Select(u => u.Id);
            UserHelper.Delete(ids);
        }
    }
}
