using Longbow.Web.Mvc;
using System;
using Xunit;

namespace Bootstrap.DataAccess.SqlServer
{
    [Collection("SQLServerContext")]
    public class ExceptionsTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            ExceptionsHelper.Log(new Exception("UnitTest"), null);
            Assert.NotEmpty(ExceptionsHelper.Retrieves());
        }

        [Fact]
        public void RetrievePages_Ok()
        {
            var op = ExceptionsHelper.RetrievePages(new PaginationOption() { Offset = 0, Limit = 20, Sort = "LogTime", Order = "desc" }, null, null);
            Assert.NotNull(op);
        }
    }
}
