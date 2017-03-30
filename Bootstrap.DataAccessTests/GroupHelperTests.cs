using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class GroupHelperTests
    {
        private Group Group { get; set; }
        private User User { get; set; }
        private Role Role { get; set; }

        [TestInitialize]
        public void IniInitialized()
        {
            Group = new Group() { GroupName = "_测试部门_", Description = "我是很厉害的测试部门" };
            User = new User() { UserName = "_测试用户_", Password = "123", PassSalt = "123", DisplayName = "测试者", RegisterTime = DateTime.Now, ApprovedTime = DateTime.Now, Description = "测试用户" };
            Role = new Role() { RoleName = "_测试角色_", Description = "测试角色" };
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "Delete from Groups where GroupName = '_测试部门_'"))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }

        [TestMethod]
        public void RetrieveGroupsTest()
        {
            Assert.IsTrue(GroupHelper.RetrieveGroups().Count() >= 0, "不带参数的GroupHelper.RetrieveGroups方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void SaveGroupTest()
        {
            // 测试插入部门方法 ID=0
            Assert.IsTrue(GroupHelper.SaveGroup(Group), "插入部门操作失败，请检查GroupHelper.SaveGroup方法");
            var groups = GroupHelper.RetrieveGroups();
            Assert.IsTrue(groups.Count() > 0, "插入部门操作失败，请检查GroupHelper.SaveGroup方法");

            //测试更新部门方法 ID!=0
            var group = groups.FirstOrDefault(g => g.GroupName == Group.GroupName);
            group.Description = "我是测试部门";
            Assert.IsTrue(GroupHelper.SaveGroup(group), string.Format("更新部门ID={0}操作失败，请检查GroupHelper.SaveGroup方法", group.Id));
            var ret = GroupHelper.RetrieveGroups(group.Id);
            Assert.IsTrue(ret.Count() == 1, "带参数的GroupHelper.RetrieveGroups方法失败");
            Assert.AreEqual(group.Description, ret.First().Description, string.Format("更新部门ID={0}操作失败，请检查GroupHelper.SaveGroup方法", group.Id));
        }

        [TestMethod]
        public void DeleteGroupTest()
        {
            // 先判断数据环境是否可以删除，没有数据先伪造数据
            var group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);
            if (group == null) GroupHelper.SaveGroup(Group);
            group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);
            Assert.IsTrue(GroupHelper.DeleteGroup(group.Id.ToString()), "GroupHelper.DeleteGroup 方法调用失败");
        }

        [TestMethod]
        public void SaveGroupsByUserIdTest()
        {
            var user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            if (user == null) UserHelper.SaveUser(User);
            user = UserHelper.RetrieveUsers().FirstOrDefault(u => u.UserName == User.UserName);
            var group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);
            if (group == null) GroupHelper.SaveGroup(Group);
            group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);

            Assert.IsTrue(GroupHelper.SaveGroupsByUserId(user.Id, group.Id.ToString()), "存储用户部门信息失败");

            Assert.IsTrue(GroupHelper.RetrieveGroupsByUserId(user.Id).Count() >= 1, string.Format("获取用户ID={0}的部门失败", user.Id));

            //删除数据
            string sql = "Delete from Users where UserName = '_测试用户_';";
            sql += "Delete from Groups where GroupName='_测试部门_';";
            sql += string.Format("Delete from UserGroup where UserID={0};", user.Id);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }

        [TestMethod]
        public void SaveGroupsByRoleIdTest()
        {
            var group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);
            if (group == null) GroupHelper.SaveGroup(Group);
            group = GroupHelper.RetrieveGroups().FirstOrDefault(g => g.GroupName == Group.GroupName);
            var role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == Role.RoleName);
            if (role == null) RoleHelper.SaveRole(Role);
            role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == Role.RoleName);

            Assert.IsTrue(GroupHelper.SaveGroupsByRoleId(role.Id, group.Id.ToString()), "存储角色部门信息失败");

            Assert.IsTrue(GroupHelper.RetrieveGroupsByRoleId(role.Id).Count() >= 1, string.Format("获取角色ID={0}的部门信息失败", role.Id));

            //删除数据
            string sql = "Delete from Groups where GroupName = '_测试部门_';";
            sql += "Delete from Roles where RoleName='_测试角色_';";
            sql += string.Format("Delete from RoleGroup where RoleID={0};", role.Id);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
    }
}
