using Longbow.Web.Mvc;
using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    public class ExceptionsTest : IClassFixture<BootstrapAdminStartup>
    {
        [Fact]
        public void Log_Ok()
        {
            Exceptions excep = new Exceptions();
            Assert.True(excep.Log(new Exception("UnitTest"), null));
        }

        [Fact]
        public void Retrieves_Ok()
        {
            Exceptions excep = new Exceptions();
            excep.Log(new Exception("UnitTest"), null);
            Assert.NotEmpty(excep.Retrieves());
        }

        [Fact]
        public void RetrievePages_Ok()
        {
            var excep = new Exceptions();
            var op = excep.RetrievePages(new PaginationOption() { Offset = 0, Limit = 20, Sort = "LogTime", Order = "desc" }, null, null);
            Assert.NotNull(op);
        }
    }
}
