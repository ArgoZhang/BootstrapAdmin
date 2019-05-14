using Longbow.Web.Mvc;
using Xunit;

namespace Bootstrap.DataAccess.SqlServer
{
    [Collection("SQLServerContext")]
    public class LogsTest
    {
        [Fact]
        public void Save_Ok()
        {
            var log = new Log()
            {
                UserName = "UnitTest",
                Browser = "UnitTest",
                City = "本地连接",
                OS = "UnitTest",
                Ip = "::1",
                CRUD = "UnitTest",
                RequestUrl = "~/Home/Index"
            };
            Assert.True(LogHelper.Save(log));
        }

        [Fact]
        public void Retrieves_Ok()
        {
            var log = new Log()
            {
                UserName = "UnitTest",
                Browser = "UnitTest",
                City = "本地连接",
                OS = "UnitTest",
                Ip = "::1",
                CRUD = "UnitTest",
                RequestUrl = "~/Home/Index"
            };
            LogHelper.Save(log);
            Assert.NotNull(LogHelper.Retrieves(new PaginationOption() { Limit = 20, Order = "LogTime" }, null, null, null));
        }
    }
}
