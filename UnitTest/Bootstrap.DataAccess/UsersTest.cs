using System;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    [Collection("SQLServerContext")]
    public class UsersTest
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Authenticate_Ok()
        {
            Assert.True(UserHelper.Authenticate("Admin", "123789", u => u.Ip = "::1"));
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Authenticate_Fail()
        {
            Assert.False(UserHelper.Authenticate("Admin-NotExists", "123789", u => u.Ip = "::1"));
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
        public void RetrieveUsersByRoleId_Ok()
        {
            var rid = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            var uid = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == "Admin").Id;

            UserHelper.SaveByRoleId(rid, new string[] { uid });

            var users = UserHelper.RetrievesByRoleId(rid);
            Assert.NotEmpty(users.Where(u => u.Checked == "checked"));
        }

        [Fact]
        public void RetrievesByGroupId_Ok()
        {
            var gid = GroupHelper.Retrieves().FirstOrDefault(r => r.GroupName == "Admin").Id;
            var users = UserHelper.RetrievesByGroupId(gid);
            Assert.NotEmpty(users.Where(u => u.Checked == "checked"));
        }

        [Fact]
        public void SaveUser_Ok()
        {
            var user = new User { UserName = "UnitTestDelete", Password = "123", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            Assert.True(UserHelper.Save(user));
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
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(g => g.RoleName == "Administrators").Id;
            var id = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == "Admin").Id;
            Assert.True(UserHelper.SaveByRoleId(roleId, new string[] { id }));
        }

        [Fact]
        public void RetrieveUserByUserName_Ok()
        {
            var usr = UserHelper.RetrieveUserByUserName("Admin");
            Assert.Equal("Administrator", usr.DisplayName);
        }
    }
}
