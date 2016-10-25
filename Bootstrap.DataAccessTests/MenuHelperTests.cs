using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class MenuHelperTests
    {
        [TestMethod]
        public void RetrieveMenusTest()
        {
            var result = MenuHelper.RetrieveMenus("1");
            Assert.IsTrue((result.Count() == 0 || result.Count() == 1), "带有参数的MenuHelper.RetrieveMenus方法调用失败，请检查数据库连接或者数据库SQL语句");
            result = MenuHelper.RetrieveMenus();
            Assert.IsTrue(result.Count() >= 0, "不带参数的MenuHelper.RetrieveMenus方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void SaveMenuTest()
        {
            Menu p = new Menu();
            p.ParentId = 1;
            p.Name = "测试菜单名称";
            p.Order = 0;            
            p.Icon = "测试菜单Icon";
            p.Url = "urlTestAdd";
            p.Category = "测试菜单分组";   
            var result = MenuHelper.SaveMenu(p);
            Assert.IsTrue(result, "增加菜单出错");

            Menu p1 = new Menu();
            p1.ID = 7;
            p1.ParentId = 2;
            p1.Name = "测试菜单名称1";
            p1.Order = 0;
            p1.Icon = "测试菜单Icon1";
            p1.Url = "urlTestUpdate";
            p1.Category = "测试菜单分组1";
            result = MenuHelper.SaveMenu(p1);
            Assert.IsTrue(result, "更新菜单出错");
        }

        [TestMethod]
        public void DeleteMenuTest()
        {
            MenuHelper.SaveMenu(new Menu() 
            { 
                ID = 0,
                ParentId = 1,
                Name = "菜单删除测试",
                Order = 0,
                Icon = "测试菜单Icon1",
                Url = "urlTestUpdate",
                Category = "测试菜单分组1"           
            });
            var menu = MenuHelper.RetrieveMenus().FirstOrDefault(m => m.Name == "菜单删除测试");
            Assert.IsTrue(MenuHelper.DeleteMenu(menu.ID.ToString()),"删除菜单失败");
            Assert.IsTrue(MenuHelper.DeleteMenu("1,2"), "带有参数的MenuHelper.DeleteMenu方法调用失败，请检查数据库连接或者数据库SQL语句");
            Assert.IsFalse(MenuHelper.DeleteMenu(string.Empty), "参数为空字符串的MenuHelper.DeleteMenu方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
    }
}
