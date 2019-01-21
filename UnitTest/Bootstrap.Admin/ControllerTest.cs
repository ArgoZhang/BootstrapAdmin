using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin
{
    [Collection("BootstrapAdminTestContext")]
    public class ControllerTest
    {
        protected HttpClient Client { get; set; }

        protected BAWebHost Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="baseAddress"></param>
        public ControllerTest(BAWebHost factory, string baseAddress = "api")
        {
            Host = factory;
            Client = factory.CreateClient(baseAddress);
        }
    }
}
