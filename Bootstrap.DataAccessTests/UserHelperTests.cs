using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            //TODO: Delete方法没有返回值，自己想想做一个返回值即可
            Assert.IsTrue(UserHelper.DeleteUser("1,2"), "带有参数的UserHelper.DeleteUserTest方法调用失败，请检查数据库连接或者数据库SQL语句");
            Assert.IsFalse(UserHelper.DeleteUser(string.Empty), "参数为空字符串的UserHelper.DeleteUserTest方法调用失败，请检查数据库连接或者数据库SQL语句");
        }

        [TestMethod]
        public void SaveUserTest()
        {
            //TODO: 两个提示一模一样完全不知道哪里出了问题，本单元测试未通过
            User users = new User();
            users.ID = 0;
            users.UserName = "liqi";
            users.Password = "123";
            users.PassSalt = "123";
            var result = UserHelper.SaveUser(users);
            Assert.IsTrue(result == true, "带有参数的UserHelper.SaveUser方法中新建用户信息失败，请检查数据库连接或者数据库SQL语句");

            User users1 = new User();
            users1.ID = 1;
            users1.UserName = "Lily";
            users1.Password = "123456";
            users1.PassSalt = "123456";
            result = UserHelper.SaveUser(users1);
            Assert.IsTrue(result == true, "带有参数的UserHelper.SaveUser方法中更新用户信息失败，请检查数据库连接或者数据库SQL语句");
        }
    }
}
