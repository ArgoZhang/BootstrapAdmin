using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class DictTest : ControllerTest
    {
        public DictTest(BAWebHost factory) : base(factory, "api/Dicts") { }

        [Theory()]
        [InlineData("Category", "asc")]
        [InlineData("Name", "asc")]
        [InlineData("Code", "asc")]
        [InlineData("Define", "asc")]
        [InlineData("Category", "desc")]
        [InlineData("Name", "desc")]
        [InlineData("Code", "desc")]
        [InlineData("Define", "desc")]
        [InlineData("", "")]
        public async void Get_Ok(string query, string order)
        {
            // 菜单 系统菜单 系统使用条件
            var qd = await Client.GetAsJsonAsync<QueryData<BootstrapDict>>($"?sort={query}&order={order}&offset=0&limit=20&category=%E8%8F%9C%E5%8D%95&name=%E7%B3%BB%E7%BB%9F%E8%8F%9C%E5%8D%95&define=0&_=1547608210979");
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<BootstrapDict, bool>("", new BootstrapDict() { Name = "UnitTest-Dict", Category = "UnitTest-Category", Code = "0", Define = 0 });
            Assert.True(ret);

            var ids = DictHelper.RetrieveDicts().Where(d => d.Name == "UnitTest-Dict").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("", ids));
        }
    }
}
