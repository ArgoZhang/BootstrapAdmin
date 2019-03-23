using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class AppsTest : ControllerTest
    {
        public AppsTest(BAWebHost factory) : base(factory, "api/Apps") { }

        [Fact]
        public async void Get_Ok()
        {
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var cates = await Client.GetAsJsonAsync<IEnumerable<App>>(rid);
            Assert.NotEmpty(cates);
        }
    }
}
