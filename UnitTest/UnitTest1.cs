using Bootstrap.Admin;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
            .UseStartup<Startup>());
        }
    }
}
