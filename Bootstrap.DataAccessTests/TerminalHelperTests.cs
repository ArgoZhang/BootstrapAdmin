using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass()]
    public class MenuTests
    {
        [TestMethod()]
        public void RetrieveMenusTest()
        {
            var result = TerminalHelper.RetrieveTerminals("1");
            Assert.IsTrue((result.Count() == 0 || result.Count() == 1), "带有参数的TerminalHelper.RetrieveTerminals方法调用失败，请检查数据库连接或者数据库SQL语句");
            result = TerminalHelper.RetrieveTerminals();
            Assert.IsTrue(result.Count() >= 0, "不带参数的TerminalHelper.RetrieveTerminals方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
    }
}