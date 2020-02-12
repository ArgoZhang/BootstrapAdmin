using Longbow.Web.Mvc;
using System;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    [Collection("Login")]
    public class UsersTest
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Authenticate_Ok()
        {
            Assert.True(UserHelper.Authenticate("Admin", "123789"));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Authenticate_Fail()
        {
            Assert.False(UserHelper.Authenticate("Admin-NotExists", "123789"));
            Assert.False(UserHelper.Authenticate("", ""));
        }

        [Fact]
        public void ChangePassword_Ok()
        {
            Assert.True(UserHelper.ChangePassword("Admin", "123789", "123789"));
        }

        [Fact]
        public virtual void Retrieves_Ok()
        {
            Assert.NotEmpty(UserHelper.Retrieves());
        }

        [Fact]
        public void RetrieveNewUsers_Ok()
        {
            UserHelper.Delete(UserHelper.RetrieveNewUsers().Select(usr => usr.Id));
            Assert.Empty(UserHelper.RetrieveNewUsers());
        }

        [Fact]
        public void Update_Ok()
        {
            var userId = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == "Admin").Id;
            Assert.True(UserHelper.Update(userId, "123789", "Administrator"));
        }

        [Fact]
        public void ApproveUser_Ok()
        {
            UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == "UnitTest").Select(usr => usr.Id));

            var up = new User() { UserName = "UnitTest", Password = "123", Description = "新建用户用于测试批准", DisplayName = "UnitTest", Icon = "default.jpg" };
            UserHelper.Save(up);
            Assert.True(UserHelper.Approve(up.Id, "UnitTest"));

            UserHelper.Delete(UserHelper.Retrieves().Where(u => u.UserName == up.UserName).Select(u => u.Id));
        }

        [Fact]
        public void SaveUser_Ok()
        {
            var user = new User { UserName = "UnitTestDelete", Password = "123", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            Assert.Equal($"{user.UserName} ({user.DisplayName})", user.ToString());
            Assert.True(UserHelper.Save(user));

            // 二次保存时返回 false
            user.Id = null;
            Assert.False(UserHelper.Save(user));
            Assert.True(UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == user.UserName).Select(usr => usr.Id)));
        }

        [Fact]
        public void SaveUserIconByName_Ok()
        {
            Assert.True(UserHelper.SaveUserIconByName("Admin", "default.jpg"));
        }

        [Fact]
        public void SaveDisplayName_Ok()
        {
            Assert.True(UserHelper.SaveDisplayName("Admin", "Administrator"));
        }

        [Fact]
        public void SaveUserCssByName_Ok()
        {
            Assert.True(UserHelper.SaveUserCssByName("Admin", "default.css"));
        }

        [Fact]
        public void Reject_Ok()
        {
            var u = new User();
            u.UserName = "UnitTestReject";
            u.DisplayName = "DisplayName";
            u.Description = "Desc";
            u.Icon = "default.jpg";
            UserHelper.Delete(UserHelper.RetrieveNewUsers().Union(UserHelper.Retrieves()).Where(usr => usr.UserName == u.UserName).Select(usr => usr.Id));
            UserHelper.Save(u);
            Assert.True(UserHelper.Reject(UserHelper.RetrieveNewUsers().FirstOrDefault(usr => usr.UserName == u.UserName).Id, "Argo"));
        }

        [Fact]
        public void SaveByGroupId_Ok()
        {
            var groupId = GroupHelper.Retrieves().FirstOrDefault(g => g.GroupName == "Admin").Id;
            var id = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == "Admin").Id;
            Assert.True(UserHelper.SaveByGroupId(groupId, new string[] { id }));

            var users = UserHelper.RetrievesByGroupId(groupId);
            Assert.NotEmpty(users.Where(u => u.Checked == "checked"));

            Assert.True(UserHelper.SaveByGroupId(groupId, new string[0]));
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(g => g.RoleName == "Administrators").Id;
            var id = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == "Admin").Id;
            Assert.True(UserHelper.SaveByRoleId(roleId, new string[] { id }));

            var users = UserHelper.RetrievesByRoleId(roleId);
            Assert.NotEmpty(users.Where(u => u.Checked == "checked"));
        }

        [Fact]
        public void RetrieveUserByUserName_Ok()
        {
            var usr = UserHelper.RetrieveUserByUserName("Admin");
            Assert.Equal("Administrator", usr.DisplayName);

            // 新建用户 默认角色为 Default
            var user = new User { UserName = "UnitTest_ICON", Password = "123", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "" };
            Assert.True(UserHelper.Save(user));
            var u = UserHelper.RetrieveUserByUserName(user.UserName);
            Assert.Equal("default.jpg", u.Icon);
            Assert.True(UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == user.UserName).Select(usr => usr.Id)));
        }

        [Fact]
        public void SaveApp_Ok()
        {
            var appId = AppHelper.RetrievesByUserName("Admin").FirstOrDefault();
            Assert.False(string.IsNullOrEmpty(appId));
            Assert.True(UserHelper.SaveApp("Admin", appId));
            UserHelper.SaveApp("Admin", "");
        }

        [Fact]
        public void ResetPassword_Ok()
        {
            Assert.False(UserHelper.ResetPassword("User", "123789"));

            var newUser = new User() { UserName = "U_Reset", DisplayName = "UnitTest", ApprovedTime = DateTime.Now, ApprovedBy = "System", Password = "1", Description = "UnitTest", RegisterTime = DateTime.Now };
            Assert.True(UserHelper.Save(newUser));
            Assert.True(UserHelper.ForgotPassword(new ResetUser() { DisplayName = "UnitTest", Reason = "UnitTest", ResetTime = DateTime.Now, UserName = newUser.UserName }));
            Assert.True(UserHelper.ResetPassword(newUser.UserName, "123"));
            Assert.True(UserHelper.Delete(new string[] { newUser.Id }));
        }

        [Fact]
        public void RetrievePageLoginUsers_Ok()
        {
            var data = LoginHelper.RetrievePages(new PaginationOption() { Limit = 20, Offset = 0, Sort = "LoginTime" }, null, null, "");
            Assert.NotNull(data.Items);
        }

        [Fact]
        public void RetrieveLoginUsers_Ok()
        {
            var data = LoginHelper.RetrieveAll(DateTime.Now.AddDays(-1), DateTime.Now, "::1");
            Assert.NotNull(data);
        }
    }
}
