using System;
using System.Threading;
using Xunit;

namespace Bootstrap.DataAccess.SQLite
{
    [Collection("SQLiteContext")]
    public class LogHelperTest
    {
        [Fact]
        public void Task_Ok()
        {
            var log = new DBLog()
            {
                Id = "",
                LogTime = DateTime.Now,
                SQL = "UnitTest",
                UserName = "Admin"
            };
            LogHelper.AddDBLog(log);
            var task = new LogHelper.DbLogTask();
            task.Execute(CancellationToken.None);
        }
    }
}
