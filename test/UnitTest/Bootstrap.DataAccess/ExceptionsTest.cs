using Longbow.Web.Mvc;
using Microsoft.Data.Sqlite;
using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class ExceptionsTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            ExceptionsHelper.Log(new Exception("UnitTest", new SqliteException("UnitTest", 1001)), null);
            Assert.NotEmpty(ExceptionsHelper.Retrieves());

            var ex = new Exceptions() { Period = "1" };
            Assert.Equal("1", ex.Period);
        }

        [Fact]
        public void RetrievePages_Ok()
        {
            Assert.NotNull(ExceptionsHelper.RetrievePages(new PaginationOption() { Offset = 0, Limit = 20, Sort = "LogTime", Order = "desc" }, null, null));
            Assert.NotNull(ExceptionsHelper.RetrievePages(new PaginationOption() { Offset = 0, Limit = 20, Sort = "ErrorPage", Order = "desc" }, null, null));
            Assert.NotNull(ExceptionsHelper.RetrievePages(new PaginationOption() { Offset = 0, Limit = 20, Sort = "UserId", Order = "desc" }, null, null));
            Assert.NotNull(ExceptionsHelper.RetrievePages(new PaginationOption() { Offset = 0, Limit = 20, Sort = "UserIp", Order = "desc" }, null, null));
            Assert.NotNull(ExceptionsHelper.RetrievePages(new PaginationOption() { Offset = 0, Limit = 20, Sort = "UserIp", Order = "desc" }, DateTime.Now.AddDays(-1), DateTime.Now));
        }
    }
}
