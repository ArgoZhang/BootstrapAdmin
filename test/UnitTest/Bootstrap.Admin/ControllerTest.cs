using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin
{
    [Collection("Login")]
    public class ControllerTest
    {
        protected HttpClient Client { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="baseAddress"></param>
        public ControllerTest(BALoginWebHost factory, string baseAddress = "api")
        {
            Client = factory.CreateClient(baseAddress);
        }
    }
}
