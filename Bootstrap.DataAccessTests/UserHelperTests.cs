using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class UserHelperTests
    {
        private User User { get; set; }
        private Role Role { get; set; }
        private Group Group { get; set; }

        [TestInitialize]
        public void Initialized()
        {
            User = new User() { UserName = "_测试用户_", Password = "123", PassSalt = "123",DisplayName="测试者" };
            Role = new Role() { RoleName = "_测试角色_", Description = "测试角色" };
            Group = new Group() { GroupName = "_测试部门_", Description = "测试部门" };
        }

        [TestCleanup]
        public void CleanUp()
        {

            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "Delete from Users where UserName = '_测试用户_'"))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }

        [TestMethod]
        public void RetrieveUsersTest()
        {
            Assert.IsTrue(UserHelper.RetrieveUsers().Count() >=1, "不带参数的UserHelper.RetrieveUsers方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void SaveUserTest()
        {
            // 测试插入用户方法 ID = 0
            Assert.IsTrue(UserHelper.SaveUser(User), "插入用户操作失败，请检查UserHelper.SaveUser 方法");
            var users = UserHelper.RetrieveUsers();
            Assert.IsTrue(users.Count() > 0, "插入用户操作失败，请检查UserHelper.SaveUser 方法");

            // 测试更新用户方法 ID != 0
            var user = users.FirstOrDefault(u => u.UserName == User.UserName);
            user.DisplayName = "测试者2号";
            Assert.IsTrue(UserHelper.SaveUser(user), string.Format("更新用户ID={0}操作失败，请检查UserHelper.SaveUser方法", user.ID));
            var ret = UserHelper.RetrieveUsers(user.ID.ToString());
            Assert.IsTrue(ret.Count() == 1, "带参数的UserHelper.RetrieveUsers方法调用失败");
            Assert.AreEqual(user.DisplayName, ret.First().DisplayName, string.Format("更新用户ID={0}操作失败，请检查UserHelper.SaveUser方法", user.ID));
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            // 先判断数据环境是否可以删除，没有数据先伪造数据
            var user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            if (user == null) UserHelper.SaveUser(User);
            user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            Assert.IsTrue(UserHelper.DeleteUser(user.ID.ToString()), "UserHelper.DeleteUserTest方法调用失败");
        }

        [TestMethod]
        public void SaveUsersByRoleIdTest()
        {
            var user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            if (user == null) UserHelper.SaveUser(User);
            user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            var role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == Role.RoleName);
            if (role == null) RoleHelper.SaveRole(Role);
            role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == Role.RoleName);

            Assert.IsTrue(UserHelper.SaveUsersByRoleId(role.ID,user.ID.ToString()), "存储角色用户信息失败");

            Assert.IsTrue(UserHelper.RetrieveUsersByRoleId(role.ID).Count()>=1, string.Format("获取角色ID={0}的用户信息失败",role.ID));

            //删除数据
            string sql = "Delete from Users where UserName = '_测试用户_';";
            sql += "Delete from Roles where RoleName='_测试角色_';";
            sql += string.Format("Delete from UserRole where RoleID={0};", role.ID);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }

        [TestMethod]
        public void SaveUsersByGroupIdTest()
        {
            var user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            if (user == null) UserHelper.SaveUser(User);
            user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            var group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);
            if (group == null) GroupHelper.SaveGroup(Group);
            group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);

            Assert.IsTrue(UserHelper.SaveUsersByGroupId(group.ID, user.ID.ToString()), "存储部门用户信息失败");

            Assert.IsTrue(UserHelper.RetrieveUsersByGroupId(group.ID).Count() >= 1, string.Format("获取部门ID={0}的用户失败", group.ID));

            //删除数据
            string sql = "Delete from Users where UserName = '_测试用户_';";
            sql += "Delete from Groups where GroupName='_测试部门_';";
            sql += string.Format("Delete from UserGroup where GroupID={0};", group.ID);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
    }
}
