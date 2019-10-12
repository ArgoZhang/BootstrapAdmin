using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class AppsTest : ControllerTest
    {
        public AppsTest(BAWebHost factory) : base(factory, "api/Apps") { }

        [Fact]
        public async void Get_Ok()
        {
            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var cates = await Client.GetAsJsonAsync<IEnumerable<App>>(rid);
            Assert.NotEmpty(cates);
        }
    }
}
