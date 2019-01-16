using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class DictsTest : ApiWebHost
    {
        public DictsTest(BAWebHost factory) : base(factory, "Dicts", true)
        {

        }

        [Fact]
        public async void Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "?sort=Category&order=asc&offset=0&limit=20&category=%E8%8F%9C%E5%8D%95&name=%E7%B3%BB%E7%BB%9F%E8%8F%9C%E5%8D%95&define=0&_=1547608210979";
            var qd = await Client.GetAsJsonAsync<QueryData<BootstrapDict>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var dict = new Dict();
            dict.Delete(new Dict().RetrieveDicts().Where(d => d.Category == "UnitTest-Category").Select(d => d.Id));

            var ret = await Client.PostAsJsonAsync<BootstrapDict, bool>("", new BootstrapDict() { Name = "UnitTest-Dict", Category = "UnitTest-Category", Code = "0", Define = 0 });
            Assert.True(ret);

            var ids = dict.RetrieveDicts().Where(d => d.Name == "UnitTest-Dict").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("", ids));
        }
    }
}
