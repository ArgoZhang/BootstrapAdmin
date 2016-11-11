using Bootstrap.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace Bootstrap.DataAccessTests
{
    [TestClass]
    public class NotificationHelperTest
    {
        [TestMethod]
        public void RetrieveNotificationsTest()
        {
            Assert.IsTrue(NotificationHelper.RetrieveNotifications("0").Count() >= 1, "带参数的NotificationHelper.RetrieveNotifications方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod]
        public void ProcessRegisterUserTest()
        {
            Assert.IsTrue(NotificationHelper.ProcessRegisterUser("1"), "带参数的NotificationHelper.ProcessRegisterUser方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
    }
}
