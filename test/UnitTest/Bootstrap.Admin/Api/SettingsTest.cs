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
