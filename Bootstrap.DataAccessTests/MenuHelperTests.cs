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

        [TestInitialize]
        public void Initialized()
        {
            Menu = new Menu() { Name = "__测试菜单__", Order = 999 };
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
        public void SaveMenuTest()
        {
            // 测试插入菜单方法 ID = 0
            Assert.IsTrue(MenuHelper.SaveMenu(Menu), "插入菜单操作失败，请检查 MenuHelper.SaveMenu 方法");
            var menus = MenuHelper.RetrieveMenus();
            Assert.IsTrue(menus.Count() > 0, "插入菜单操作失败，请检查 MenuHelper.SaveMenu 方法");

            // 测试更新菜单方法 ID != 0
            var menu = menus.FirstOrDefault(m => m.Name == Menu.Name);
            menu.Icon = "fa";
            Assert.IsTrue(MenuHelper.SaveMenu(menu), string.Format("更新菜单ID = {0} 操作失败，请检查 MenuHelper.SaveMenu 方法", menu.ID));
            var dest = MenuHelper.RetrieveMenus(menu.ID.ToString());
            Assert.IsTrue(dest.Count() == 1, "带参数的MenuHelper.RetrieveMenus方法调用失败");
            Assert.AreEqual(menu.Icon, dest.First().Icon, string.Format("更新菜单ID = {0} 操作失败，请检查 MenuHelper.SaveMenu 方法", menu.ID));
        }

        [TestMethod]
        public void DeleteMenuTest()
        {
            // 先判断数据环境是否可以删除，没有数据先伪造数据
            var menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);
            if (menu == null) MenuHelper.SaveMenu(Menu);
            menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == Menu.Name);
            Assert.IsTrue(MenuHelper.DeleteMenu(menu.ID.ToString()), "MenuHelper.DeleteMenu 方法调用失败");
        }

        [TestMethod()]
        public void RetrieveMenusByUserIdTest()
        {
            // UNDONE: 根据代码编写单元测试
            Assert.IsTrue(true);
        }
    }
}
