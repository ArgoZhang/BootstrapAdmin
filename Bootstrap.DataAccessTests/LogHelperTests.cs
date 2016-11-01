using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class LogHelperTests
    {
        [TestMethod]
        public void RetrieveLogsTest()
        {
            var result = LogHelper.RetrieveLogs("1");
            Assert.IsTrue((result.Count() == 0 || result.Count() == 1), "带有参数的LogHelper.RetrieveLogs方法调用失败，请检查数据库连接或者数据库SQL语句");
            result = LogHelper.RetrieveLogs();
            Assert.IsTrue(result.Count() >= 0, "带有参数的LogHelper.RetrieveLogs方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod]
        public void saveLogTest()
        {
            Log p = new Log();
            p.OperationType = 1;
            p.UserID = 1;
            p.OperationTime = System.DateTime.Now;
            p.TableName = "日志";
            p.BusinessName = "新增日志信息";
            p.PrimaryKey = "ID";
            p.SqlText = "Insert Into Logs";
            p.OperationIp = "0.0.0.0";

            var result = LogHelper.saveLog(p);
            Assert.IsTrue(result, "新增日志信息出错");
        }
        [TestMethod]
        public void DeleteLogTest()
        {
            string p = "2";
            Assert.IsTrue(LogHelper.DeleteLog(p),"删除日志信息出错");
        }
    }
}
