using Xunit;

namespace Bootstrap.DataAccess
{
    public class LogsTest : IClassFixture<BootstrapAdminStartup>
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
            var log = new Log();
            Assert.NotEmpty(log.Retrieves());
        }
    }
}
