using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.Admin.Api
{
    public class SettingsTest : ControllerTest
    {
        public SettingsTest(BALoginWebHost factory) : base(factory, "api/Settings") { }

        [Fact]
        public async void Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<CacheCorsItem>>();
            Assert.NotNull(resp);
        }

        [Fact]
        public async void GetByKey_Ok()
        {
            var resp = await Client.GetAsJsonAsync<QueryAppOption>("Demo");
            Assert.NotNull(resp);
        }

        [Fact]
        public async void Put_Ok()
        {
            var data = new QueryAppOption()
            {
                AppId = "new",
                AppName = "UnitTest_Put",
                AppCode = "UnitTest_Put",
                AppUrl = "http://localhost",
                AppTitle = "网站标题",
                AppFooter = "网站页脚"
            };

            var resp = await Client.PutAsJsonAsync<QueryAppOption, bool>("", data);
            Assert.True(resp);

            // Check
            var op = await Client.GetAsJsonAsync<QueryAppOption>(data.AppCode);
            Assert.Equal(data.AppTitle, op.AppTitle);

            // update
            data.AppId = "edit";
            data.AppUrl = "http://UnitTest";
            resp = await Client.PutAsJsonAsync<QueryAppOption, bool>("", data);
            Assert.True(resp);

            op = await Client.GetAsJsonAsync<QueryAppOption>(data.AppCode);
            Assert.Equal(data.AppUrl, op.AppUrl);

            // 删除
            resp = await Client.DeleteAsJsonAsync<BootstrapDict, bool>("AppPath", new BootstrapDict()
            {
                Category = data.AppName,
                Name = data.AppName,
                Code = data.AppCode
            });
            Assert.True(resp);
        }

        [Fact]
        public async void Post_Ok()
        {
            var dicts = DictHelper.RetrieveDicts();

            var ids = dicts.Where(d => d.Category == "UnitTest-Settings").Select(d => d.Id);
            DictHelper.Delete(ids);

            Assert.True(DictHelper.Save(new BootstrapDict() { Category = "UnitTest-Settings", Name = "UnitTest", Code = "0", Define = 0 }));

            // 调用 Settings webapi
            var resp = await Client.PostAsJsonAsync<IEnumerable<BootstrapDict>, bool>("", new BootstrapDict[]{
                new BootstrapDict() { Category = "UnitTest-Settings", Name = "UnitTest", Code = "UnitTest" }
            });
            Assert.True(resp);

            // 由于 SaveUISettings 函数保护功能，上一步保存成功，但是未更改 Code 值
            var code = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "UnitTest-Settings").Code;
            Assert.Equal("0", code);

            // Delete
            ids = DictHelper.RetrieveDicts().Where(d => d.Category == "UnitTest-Settings").Select(d => d.Id);
            DictHelper.Delete(ids);
        }

        [Fact]
        public async void Post_Id_Ok()
        {
            // Demo
            var resp = await Client.PostAsJsonAsync<BootstrapDict, bool>("Demo", new BootstrapDict() { Name = "UnitTest", Code = "0" });
            Assert.False(resp);

            resp = await Client.PostAsJsonAsync<BootstrapDict, bool>("Demo", new BootstrapDict() { Name = "123789", Code = "0" });
            Assert.True(resp);
        }

        internal class CacheCorsItem : ICacheCorsItem
        {
            /// <summary>
            ///
            /// </summary>
            public bool Enabled { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string Desc { get; set; }

            /// <summary>
            ///
            /// </summary>
            public bool Self { get; set; }
        }
    }
}
