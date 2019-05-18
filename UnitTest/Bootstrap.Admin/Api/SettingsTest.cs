using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bootstrap.Admin.Api.SqlServer
{
    public class SettingsTest : ControllerTest
    {
        public SettingsTest(BAWebHost factory) : base(factory, "api/Settings") { }

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

            // 获得原来值
            var resp = await Client.PostAsJsonAsync<BootstrapDict, bool>(new BootstrapDict() { Category = "UnitTest-Settings", Name = "UnitTest", Code = "UnitTest" });
            Assert.True(resp);

            var code = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "UnitTest-Settings").Code;
            Assert.Equal("UnitTest", code);

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
