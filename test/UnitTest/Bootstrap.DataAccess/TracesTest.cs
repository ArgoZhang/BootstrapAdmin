using Longbow.Web.Mvc;
using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class TracesTest
    {
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
            Assert.True(DbContextManager.Create<Trace>().Save(log));
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "LogTime" }, null, null, null).Items);
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "IP" }, null, null, null).Items);
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "UserName" }, null, null, null).Items);
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "City" }, null, null, null).Items);
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "Browser" }, null, null, null).Items);
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "OS" }, null, null, null).Items);
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "RequestUrl" }, null, null, null).Items);
            Assert.NotNull(TraceHelper.Retrieves(new PaginationOption() { Limit = 20, Offset = 0, Order = "desc", Sort = "RequestUrl" }, DateTime.Now.AddDays(-1), DateTime.Now, "::1").Items);
            Assert.NotEmpty(TraceHelper.RetrieveAll(DateTime.Now.AddDays(-1), DateTime.Now, "::1"));
        }
    }
}
