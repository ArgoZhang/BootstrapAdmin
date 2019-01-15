using Microsoft.AspNetCore.Mvc.Testing;
using UnitTest;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class BAWebHost : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// 
        /// </summary>
        public BAWebHost()
        {
            // Copy license
            TestHelper.CopyLicense();
        }
    }
}
