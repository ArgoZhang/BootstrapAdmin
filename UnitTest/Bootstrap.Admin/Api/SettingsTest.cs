using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class SettingsTest : ApiTest
    {
        public SettingsTest(BAWebHost factory) : base(factory, "Settings", true)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<ICacheCorsItem>>();
            Assert.NotNull(resp);
        }

        [Fact]
        public async void Post_Ok()
        {
            var dict = new Dict();
            var dicts = dict.RetrieveDicts();

            var ids = dicts.Where(d => d.Category == "UnitTest-Settings").Select(d => d.Id);
            dict.Delete(ids);

            Assert.True(dict.Save(new Dict() { Category = "UnitTest-Settings", Name = "UnitTest", Code = "0", Define = 0 }));

            // 获得原来值
            var resp = await Client.PostAsJsonAsync<BootstrapDict, bool>("", new Dict() { Category = "UnitTest-Settings", Name = "UnitTest", Code = "UnitTest" });
            Assert.True(resp);

            var code = dict.RetrieveDicts().FirstOrDefault(d => d.Category == "UnitTest-Settings").Code;
            Assert.Equal("UnitTest", code);

            // Delete 
            ids = dict.RetrieveDicts().Where(d => d.Category == "UnitTest-Settings").Select(d => d.Id);
            dict.Delete(ids);
        }
    }
}
