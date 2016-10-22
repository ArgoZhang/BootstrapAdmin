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
            string ids = "2";
            try
            {
                UserHelper.DeleteUser(ids);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false,"带有参数的UserHelper.DeleteUserTest方法调用失败，请检查数据库连接或者数据库SQL语句");
            }
            
        }

        [TestMethod]
        public void SaveUserTest()
        {
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
