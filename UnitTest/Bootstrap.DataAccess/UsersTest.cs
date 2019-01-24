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
            var u = new User();
            Assert.True(u.Authenticate("Admin", "123789"));
        }

        [Fact]
        public void ChangePassword_Ok()
        {
            var u = new User();
            Assert.True(u.ChangePassword("Admin", "123789", "123789"));
        }

        [Fact]
        public virtual void Retrieves_Ok()
        {
            var u = new User();
            Assert.NotEmpty(u.Retrieves());
        }

        [Fact]
        public void RetrieveNewUsers_Ok()
        {
            var u = new User();
            u.Delete(u.RetrieveNewUsers().Select(usr => usr.Id));
            Assert.Empty(u.RetrieveNewUsers());
        }

        [Fact]
        public void Update_Ok()
        {
            var u = new User();
            Assert.True(u.Update("1", "123789", "Administrator"));
        }

        [Fact]
        public void ApproveUser_Ok()
        {
            var u = new User();
            u.Delete(u.Retrieves().Where(usr => usr.UserName == "UnitTest").Select(usr => usr.Id));

            var up = new User() { UserName = "UnitTest", Password = "123", Description = "新建用户用于测试批准", DisplayName = "UnitTest", Icon = "default.jpg" };
            u.Save(up);
            Assert.True(u.Approve(up.Id, "UnitTest"));

            u.Delete(new string[] { up.Id });
        }

        [Fact]
        public void RetrieveUsersByRoleId_Ok()
        {
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var db = DbManager.Create();
            db.Execute("delete from userrole where USERID = @0 and ROLEID = @1", uid, rid);
            db.Execute("insert into userrole (USERID, ROLEID) values (@0, @1)", uid, rid);

            var users = new User().RetrievesByRoleId(rid);
            Assert.NotEmpty(users);
            Assert.Contains(users, usr => usr.Checked == "checked");
        }

        [Fact]
        public void RetrievesByGroupId_Ok()
        {
            var gid = new Group().Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            var u = new User();
            var users = u.RetrievesByGroupId(gid);
            Assert.NotEmpty(users);
            Assert.Contains(users, usr => !usr.Checked.IsNullOrEmpty());
        }

        [Fact]
        public void DeleteUser_Ok()
        {
            var u = new User();
            Assert.True(u.Delete(new string[] { "5", "6" }));
        }

        [Fact]
        public void SaveUser_Ok()
        {
            var u = new User();
            u.Delete(u.Retrieves().Where(usr => usr.UserName == "UnitTest").Select(usr => usr.Id));
            Assert.True(u.Save(new User { UserName = "UnitTest", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" }));
            u.Delete(u.Retrieves().Where(usr => usr.UserName == "UnitTest").Select(usr => usr.Id));
        }

        [Fact]
        public void SaveUserIconByName_Ok()
        {
            var u = new User();
            Assert.True(u.SaveUserIconByName("Admin", "default.jpg"));
        }

        [Fact]
        public void SaveDisplayName_Ok()
        {
            var u = new User();
            Assert.True(u.SaveDisplayName("Admin", "Administrator"));
        }

        [Fact]
        public void SaveUserCssByName_Ok()
        {
            var u = new User();
            Assert.True(u.SaveUserCssByName("Admin", "default.css"));
        }

        [Fact]
        public void Reject_Ok()
        {
            var u = new User();
            u.UserName = "UnitTest-Reject";
            u.DisplayName = "DisplayName";
            u.Description = "Desc";
            u.Icon = "default.jpg";
            u.Save(u);
            Assert.True(u.Reject(u.Id, "Argo"));
        }

        [Fact]
        public void SaveByGroupId_Ok()
        {
            var u = new User();
            Assert.True(u.SaveByGroupId("1", new string[] { "1" }));
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            var u = new User();
            Assert.True(u.SaveByRoleId("1", new string[] { "1", "2" }));
        }

        [Fact]
        public void RetrieveUserByUserName_Ok()
        {
            var u = new User();
            var usr = u.RetrieveUserByUserName("Admin");
            Assert.Equal("Administrator", usr.DisplayName);
        }
    }
}
