using Longbow.Web.Mvc;
using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class TracesTest
    {
        [Fact]
        public void Save_Ok()
        {
            var log = new Trace()
            {
                UserName = "UnitTest",
                Browser = "UnitTest",
                City = "本地连接",
                OS = "UnitTest",
                Ip = "::1",
                LogTime = DateTime.Now,
                RequestUrl = "~/Home/Index"
            };
            Assert.True(log.Save(log));
        }

        [Fact]
        public void Retrieves_Ok()
        {
            var log = new Trace()
            {
                UserName = "UnitTest",
                Browser = "UnitTest",
                City = "本地连接",
                OS = "UnitTest",
                Ip = "::1",
                LogTime = DateTime.Now,
                RequestUrl = "~/Home/Index"
            };
            log.Save(log);
            Assert.NotEmpty(log.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "LogTime" }, null, null).Items);
        }
    }
}
