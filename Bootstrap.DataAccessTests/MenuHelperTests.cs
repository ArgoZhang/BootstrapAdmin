using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class MenuHelperTests
    {
        private Menu Menu { get; set; }
        private Role Role { get; set; }

        [TestInitialize]
        public void Initialized()
        {
            Menu = new Menu() { Name = "__测试菜单__", Order = 999, Category = "0" };
            Role = new Role() { RoleName = "_测试角色_", Description = "这是一个测试角色", Checked = "0" };
        }
        [TestCleanup]
        public void CleanUp()
        {
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "Delete from Navigations where Name = '__测试菜单__'"))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }

        [TestMethod]
        public void RetrieveMenusTest()
        {
            Assert.IsTrue(MenuHelper.RetrieveMenus().Count() > 1, "不带参数的MenuHelper.RetrieveMenus方法调用失败");
        }

        [TestMethod]
        public void RetrieveMenuByRoleIDTest()
        {
            Assert.IsTrue(MenuHelper.RetrieveMenusByRoleId(1).Count() >= 0, "根据角色ID查询菜单的MenuHelper.RetrieveMenusByRoleId方法调用失败");
        }

        [TestMethod]
        public void SaveMenuTest()
        {
            // 测试插入菜单方法 ID = 0
            Assert.IsTrue(MenuHelper.SaveMenu(Menu), "插入菜单操作失败，请检查 MenuHelper.SaveMenu 方法");
            var menus = MenuHelper.RetrieveMenus();
            Assert.IsTrue(menus.Count() > 0, "插入菜单操作失败，请检查 MenuHelper.SaveMenu 方法");

            // 测试更新菜单方法 ID != 0
            var menu = menus.FirstOrDefault(m => m.Name == Menu.Name);
            menu.Icon = "fa";
            Assert.IsTrue(MenuHelper.SaveMenu(menu), string.Format("更新菜单ID = {0} 操作失败，请检查 MenuHelper.SaveMenu 方法", menu.Id));
        }

        [TestMethod]
        public void DeleteMenuTest()
        {
            // 先判断数据环境是否可以删除，没有数据先伪造数据
            var menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);
            if (menu == null) MenuHelper.SaveMenu(Menu);
            menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);
            Assert.IsTrue(MenuHelper.DeleteMenu(menu.Id.ToString()), "MenuHelper.DeleteMenu 方法调用失败");
        }

        [TestMethod]
        public void SavaRolesByMenuIdTest()
        {
            var menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);
            if (menu == null) MenuHelper.SaveMenu(Menu);
            menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);

            var role = RoleHelper.RetrieveRoles().FirstOrDefault(m => m.RoleName == Role.RoleName);
            if (role == null) RoleHelper.SaveRole(Role);
            role = RoleHelper.RetrieveRoles().FirstOrDefault(m => m.RoleName == Role.RoleName);

            Assert.IsTrue(RoleHelper.SavaRolesByMenuId(menu.Id, role.Id.ToString()), "保存菜单角色关系失败");
            Assert.IsTrue(RoleHelper.RetrieveRolesByMenuId(menu.Id).Count() > 0, string.Format("获取菜单ID={0}的角色关系失败", menu.Id));

            //删除数据
            string sql = "delete from Navigations where Name='__测试菜单__';";
            sql += "delete from Roles where RoleName='_测试角色_';";
            sql += string.Format("delete from NavigationRole where NavigationID={0}", menu.Id);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
        [TestMethod]
        public void SaveMenusByRoleIdTest()
        {
            var menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);
            if (menu == null) MenuHelper.SaveMenu(Menu);
            menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);
            var role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == Role.RoleName);
            if (role == null) RoleHelper.SaveRole(Role);
            role = RoleHelper.RetrieveRoles().FirstOrDefault(r => r.RoleName == Role.RoleName);

            Assert.IsTrue(MenuHelper.SaveMenusByRoleId(role.Id, menu.Id.ToString()), "存储角色菜单信息失败");
            int x = MenuHelper.RetrieveMenusByRoleId(role.Id).Count();
            Assert.IsTrue(x >= 1, string.Format("获取角色ID={0}的菜单信息失败", role.Id));

            //删除数据
            string sql = "Delete from Navigations where Name = '__测试菜单__';";
            sql += "Delete from Roles where RoleName='_测试角色_';";
            sql += string.Format("Delete from NavigationRole where RoleID={0};", role.Id);
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
    }
}
