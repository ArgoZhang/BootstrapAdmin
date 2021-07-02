using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class AppsTest : ControllerTest
    {
        public AppsTest(BALoginWebHost factory) : base(factory, "api/Apps") { }

        [Fact]
        public async void Get_Ok()
        {
            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var cates = await Client.GetFromJsonAsync<IEnumerable<App>>(rid);
            Assert.NotEmpty(cates);
        }
    }
}
