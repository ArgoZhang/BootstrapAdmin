using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class RoleTests
    {
        private Role Role { get; set; }
        private User User { get; set; }
        private Group Group { get; set; }
        [TestInitialize]
        public void Initialized()
        {
            Role = new Role() { RoleName = "_测试角色_", Description = "这是一个测试角色", Checked = "0" };
            User = new User() { UserName = "_测试用户_", Password = "111", PassSalt = "111", DisplayName = "_测试用户_", Checked = "0", RegisterTime = DateTime.Now, ApprovedTime = DateTime.Now, Description = "测试用户" };
            Group = new Group() { GroupName = "_测试部门_", Description = "这是一个测试部门", Checked = "0" };
        }
        [TestCleanup]
        public void CleanUp()
        {
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "delete from Roles where RoleName='_测试角色_'"))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
        [TestMethod]
        public void SaveRoleTest()
        {
            //测试添加角色方法 ID = 0
            Assert.IsTrue(RoleHelper.SaveRole(Role), "添加角色操作失败，请检查RoleHelper.SaveRole方法");
            var roles = RoleHelper.RetrieveRoles();
            Assert.IsTrue(roles.Count() > 0, "添加角色操作失败，请检查RoleHelper.SaveRole方法");
            //测试编辑角色方法 ID != 0
            var role = roles.FirstOrDefault(m => m.RoleName == Role.RoleName);
            role.Description = "这是修改后的测试角色";
            Assert.IsTrue(RoleHelper.SaveRole(role), string.Format("更新角色ID={0}操作失败，请检查RoleHelper.SaveRole方法", role.ID));
            var ret = RoleHelper.RetrieveRoles(role.ID);
            Assert.IsTrue(ret.Count() == 1, "带参数的RoleHelper.RetrieveRoles方法调用失败");
            Assert.AreEqual(role.Description, ret.First().Description, string.Format("更新角色ID={0}操作失败，请检查RoleHelper.SaveRole方法", role.ID));
        }
        [TestMethod]
        public void RetrieveRoleTest()
        {
            var result = RoleHelper.RetrieveRoles();
            Assert.IsTrue(result.Count() >= 0, "不带参数的RoleHelper.RetrieveRole方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod]
        public void DeleteRoleTest()
        {

            var role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == "_测试角色_");
            if (role == null) RoleHelper.SaveRole(Role);
            role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == "_测试角色_");
            Assert.IsTrue(RoleHelper.DeleteRole(role.ID.ToString()), "删除角色失败");
        }
        [TestMethod]
        public void SaveRolesByUserIdTest()
        {
            var role = RoleHelper.RetrieveRoles().FirstOrDefault(m => m.RoleName == Role.RoleName);
            if (role == null) RoleHelper.SaveRole(Role);
            role = RoleHelper.RetrieveRoles().FirstOrDefault(m => m.RoleName == Role.RoleName);

            var user = UserHelper.RetrieveUsers().FirstOrDefault(m => m.DisplayName == User.DisplayName);
            if (user == null) UserHelper.SaveUser(User);
            user = UserHelper.RetrieveUsers().FirstOrDefault(m => m.DisplayName == User.DisplayName);

            Assert.IsTrue(RoleHelper.SaveRolesByUserId(user.ID, role.ID.ToString()), "保存用户角色关系失败");
            Assert.IsTrue(RoleHelper.RetrieveRolesByUserId(user.ID).Count() >= 1, string.Format("获取用户ID={0}的角色信息失败", user.ID));
            //删除数据
            string sql = "delete from Roles where RoleName='_测试角色_';";
            sql += "delete from Users where UserName='_测试用户_';";
            sql += string.Format("delete from UserRole where UserID={0}", user.ID);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
        [TestMethod]
        public void SaveRoleByGroupIDTest()
        {
            var role = RoleHelper.RetrieveRoles().FirstOrDefault(m => m.RoleName == Role.RoleName);
            if (role == null) RoleHelper.SaveRole(Role);
            role = RoleHelper.RetrieveRoles().FirstOrDefault(m => m.RoleName == Role.RoleName);

            var group = GroupHelper.RetrieveGroups().FirstOrDefault(m => m.GroupName == Group.GroupName);
            if (group == null) GroupHelper.SaveGroup(Group);
            group = GroupHelper.RetrieveGroups().FirstOrDefault(m => m.GroupName == Group.GroupName);

            Assert.IsTrue(RoleHelper.SaveRolesByGroupId(group.ID, role.ID.ToString()), "保存部门角色关系失败");
            Assert.IsTrue(RoleHelper.RetrieveRolesByGroupId(group.ID).Count() > 0, string.Format("获取部门ID={0}的角色关系失败", group.ID));
            //删除数据
            string sql = "delete from Roles where RoleName='_测试角色_';";
            sql += "delete from Groups where GroupName='_测试部门_';";
            sql += string.Format("delete from RoleGroup where GroupID={0}", group.ID);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
    }
}
