using Xunit;

namespace Bootstrap.DataAccess
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
                ClientAgent = "UnitTest-Agent",
                ClientIp = "::",
                CRUD = "UnitTest",
                RequestUrl = "~/Home/Index"
            };
            Assert.True(log.Save(log));
        }

        [Fact]
        public void Retrieves_Ok()
        {
            var log = new Log()
            {
                UserName = "UnitTest",
                ClientAgent = "UnitTest-Agent",
                ClientIp = "::",
                CRUD = "UnitTest",
                RequestUrl = "~/Home/Index"
            };
            log.Save(log);
            Assert.NotEmpty(log.Retrieves());
        }
    }
}
