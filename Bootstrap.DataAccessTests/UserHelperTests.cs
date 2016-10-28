using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class UserHelperTests
    {
        [TestMethod]
        public void RetrieveUsersTest()
        {
            var result = UserHelper.RetrieveUsers("1");
            Assert.IsTrue((result.Count() == 0 || result.Count() == 1), "带有参数的UserHelper.RetrieveUsers方法调用失败，请检查数据库连接或者数据库SQL语句");
            result = UserHelper.RetrieveUsers();
            Assert.IsTrue(result.Count() >= 0, "不带参数的UserHelper.RetrieveUsers方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            Assert.IsTrue(UserHelper.DeleteUser("1,2"), "带有参数的UserHelper.DeleteUserTest方法调用失败，请检查数据库连接或者数据库SQL语句");
            Assert.IsFalse(UserHelper.DeleteUser(string.Empty), "参数为空字符串的UserHelper.DeleteUserTest方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void SaveUserTest()
        {
            User users = new User();
            users.ID = 0;
            users.UserName = "lq";
            users.Password = "123";
            users.PassSalt = "123";
            users.DisplayName = "liqi";
            var result = UserHelper.SaveUser(users);
            Assert.IsTrue(result == true, "新建用户信息失败，请检查数据库连接或者数据库SQL语句");

            User users1 = new User();
            users1.ID = 5;
            users1.UserName = "lq";
            users1.Password = "123";
            users1.PassSalt = "123456";
            users1.DisplayName = "Qi Li";
            result = UserHelper.SaveUser(users1);
            Assert.IsTrue(result == true, "更新用户信息失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod]
        public void RetrieveUsersByRoleIdTest(){
            IEnumerable<User> result = UserHelper.RetrieveUsersByRoleId(2);
            Assert.IsTrue(result.Count() >= 0, "获取该角色的用户信息失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod]
        public void SaveUsersByRoleIdTest()
        {
            bool result = UserHelper.SaveUsersByRoleId(2,"2,3");
            Assert.IsTrue(result, "获取该角色的用户信息失败，请检查数据库连接或者数据库SQL语句");
        }
    }
}
