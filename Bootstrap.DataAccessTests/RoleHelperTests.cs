using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass()]
    public class RoleTests
    {
        [TestMethod()]
        public void SaveRoleTest()
        {
            Role role1 = new Role()
            {
                RoleName = "管理员",
                Description = "可以读写所有内容"
            };
            var result1 = RoleHelper.SaveRole(role1);
            Assert.IsTrue(result1 == true, "带有参数的RoleHelper.SaveRole方法添加用户失败，请检查数据库连接或者数据库SQL语句");
            Role role2 = new Role()
            {
                ID = 1,
                RoleName = "管理员",
                Description = "读写所有内容"
            };
            var result2 = RoleHelper.SaveRole(role2);
            Assert.IsTrue(result2 == true, "带有参数的RoleHelper.SaveRole方法编辑用户信息失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod()]
        public void RetrieveRoleTest()
        {
            var result = RoleHelper.RetrieveRole("1");
            Assert.IsTrue((result.Count() == 0 || result.Count() == 1), "带有参数的RoleHelper.RetrieveRole方法调用失败，请检查数据库连接或者数据库SQL语句");
            result = RoleHelper.RetrieveRole();
            Assert.IsTrue(result.Count() >= 0, "不带参数的RoleHelper.RetrieveRole方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod()]
        public void DeleteRoleTest()
        {
            RoleHelper.SaveRole(new Role()
            {
                ID = 0,
                RoleName = "RoleUnitTest",
                Description = string.Empty
            });
            var role = RoleHelper.RetrieveRole().FirstOrDefault(r => r.RoleName == "RoleUnitTest");
            Assert.IsTrue(RoleHelper.DeleteRole(role.ID.ToString()), "删除用户失败");
        }
    }
}
