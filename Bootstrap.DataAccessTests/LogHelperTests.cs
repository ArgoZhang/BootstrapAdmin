using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.Tests
{
    [TestClass]
    public class LogHelperTests
    {
        private Log Log { get; set; }
        [TestInitialize]
        public void Initialized()
        {
            Log = new Log() { OperationType = 1, UserID = 1, OperationTime = System.DateTime.Now, TableName = "_测试日志_", BusinessName = "新增测试日志信息", PrimaryKey = "ID", SqlText = "Insert Into Logs", OperationIp = "0.0.0.0" };
        }
        [TestCleanup]
        public void CleanUp()
        {
            using (DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, "delete from Logs where OperationIp='0'"))
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
            Assert.IsTrue(LogHelper.SaveLog(Log), "新增日志信息出错,请检查LogHelper的saveLog 方法");
            var logs = LogHelper.RetrieveLogs();
            Assert.IsTrue(logs.Count() > 0, "新增日志信息出错,请检查LogHelper的saveLog 方法");
        }
        [TestMethod]
        public void DeleteLogTest()
        {
            // 先判断数据环境是否可以删除，没有数据先伪造数据
            var log = LogHelper.RetrieveLogs().FirstOrDefault(m => m.OperationType == Log.OperationType);
            if (log == null) LogHelper.SaveLog(Log);
            log = LogHelper.RetrieveLogs().FirstOrDefault(m => m.OperationType == Log.OperationType);
            Assert.IsTrue(LogHelper.DeleteLog(log.ID.ToString()), "删除日志信息出错");
        }
    }
}
