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
        public async void Post_Ok()
        {
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var cates = await Client.PostAsJsonAsync<string, IEnumerable<App>>(rid, string.Empty);
            Assert.NotEmpty(cates);
        }

        [Fact]
        public async void Put_Ok()
        {
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>(rid, new string[] { "1", "2" });
            Assert.True(ret);
        }
    }
}
