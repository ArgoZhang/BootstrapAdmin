using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Data;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class LogHelperTests
    {
        private Log Log { get; set; }
        [TestInitialize]
        public void Initialized()
        {
            Log = new Log() { OperationType = 1, UserName = "_测试用户名称_", OperationTime = System.DateTime.Now, OperationIp = "0.0.0.0",Remark="" };
        }
        [TestCleanup]
        public void CleanUp()
        {
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "delete from Logs where UserName='_测试用户名称_'"))
            {
                DBAccessManager.SqlDBAccess.ExecuteNonQuery(cmd);
            }
        }
        [TestMethod]
        public void RetrieveLogsTest()
        {
            Assert.IsTrue(LogHelper.RetrieveLogs().Count() >= 0, "带有参数的LogHelper.RetrieveLogs方法调用失败，请检查数据库连接或者数据库SQL语句");
        }
        [TestMethod]
        public void SaveLogTest()
        {
            Assert.IsTrue(LogHelper.SaveLog(Log), "新增日志信息出错,请检查LogHelper的SaveLog 方法");
            var logs = LogHelper.RetrieveLogs();
            Assert.IsTrue(logs.Count() > 0, "新增日志信息出错,请检查LogHelper的SaveLog 方法");
        }
        [TestMethod]
        public void DeleteLogTest()
        {
            // 先判断数据环境是否可以删除，没有数据先伪造数据
            var log = LogHelper.RetrieveLogs().FirstOrDefault(l => l.UserName == Log.UserName);
            if (log == null) LogHelper.SaveLog(Log);
            log = LogHelper.RetrieveLogs().FirstOrDefault(l => l.UserName == Log.UserName);
            Assert.IsTrue(LogHelper.DeleteLog(log.ID.ToString()), "删除日志信息出错");
        }
    }
}
