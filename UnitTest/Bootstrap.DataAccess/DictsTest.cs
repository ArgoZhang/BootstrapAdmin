using Bootstrap.Security;
using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.DataAccess.SqlServer
{
    [Collection("SQLServerContext")]
    public class DictsTest
    {
        protected virtual string DatabaseName { get; set; } = "SQLServer";

        [Fact]
        public void SaveAndDelete_Ok()
        {
            var dict = new BootstrapDict()
            {
                Category = "UnitTest",
                Name = "SaveDict",
                Code = "1",
                Define = 1
            };
            Assert.True(DictHelper.Save(dict));
            dict.Code = "2";
            Assert.True(DictHelper.Save(dict));
            Assert.True(DictHelper.Delete(new string[] { dict.Id }));
        }

        [Fact]
        public void SaveSettings_Ok()
        {
            var dict = new Dict()
            {
                Category = "UnitTest",
                Name = "SaveSettings",
                Code = "1",
                Define = 1
            };

            // insert 
            Assert.True(DictHelper.Save(dict));
            // update
            Assert.True(DictHelper.SaveSettings(dict));
            // delete
            Assert.True(DictHelper.Delete(new string[] { dict.Id }));
        }

        [Fact]
        public void RetrieveCategories_Ok()
        {
            Assert.NotEmpty(DictHelper.RetrieveCategories());
        }

        [Fact]
        public void RetrieveWebTitle_Ok()
        {
            Assert.Equal("后台管理系统", DictHelper.RetrieveWebTitle());
        }

        [Fact]
        public void RetrieveWebFooter_Ok()
        {
            Assert.Equal("2016 © 通用后台管理系统", DictHelper.RetrieveWebFooter());
        }

        [Fact]
        public void RetrieveThemes_Ok()
        {
            Assert.NotEmpty(DictHelper.RetrieveThemes());
        }

        [Fact]
        public void RetrieveActiveTheme_Ok()
        {
            Assert.Equal("blue.css", DictHelper.RetrieveActiveTheme());
        }

        [Fact]
        public void RetrieveIconFolderPath_Ok()
        {
            Assert.Equal("~/images/uploader/", DictHelper.RetrieveIconFolderPath());
        }

        [Fact]
        public void RetrieveHomeUrl_Ok()
        {
            Assert.Equal("~/Home/Index", DictHelper.RetrieveHomeUrl("0"));
            var url = DictHelper.RetrieveHomeUrl("2");
            Assert.NotEqual("~/Home/Index", url);

            // INSERT INTO [Dicts] ([Category], [Name], [Code], [Define]) VALUES ('应用首页', 2, 'http://localhost:49185/', 0);
            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "应用首页" && d.Name == "2");
            url = dict.Code;
            dict.Code = "";
            Assert.True(DictHelper.Save(dict));
            Assert.Equal("~/Home/Index", DictHelper.RetrieveHomeUrl("2"));

            dict.Code = url;
            Assert.True(DictHelper.Save(dict));
        }

        [Fact]
        public void RetrieveApps_Ok()
        {
            Assert.NotEmpty(DictHelper.RetrieveApps());
        }

        [Fact]
        public void RetrieveDicts_Ok()
        {
            Assert.NotEmpty(DictHelper.RetrieveDicts());
        }

        [Fact]
        public void RetrieveCookieExpiresPeriod_Ok()
        {
            Assert.Equal(7, DictHelper.RetrieveCookieExpiresPeriod());
        }

        [Fact]
        public void RetrieveExceptionsLogPeriod_Ok()
        {
            Assert.Equal(1, DictHelper.RetrieveExceptionsLogPeriod());
        }

        [Fact]
        public void RetrieveLoginLogsPeriod_Ok()
        {
            Assert.Equal(12, DictHelper.RetrieveLoginLogsPeriod());
        }

        [Fact]
        public void RetrieveLogsPeriod_Ok()
        {
            Assert.Equal(12, DictHelper.RetrieveLogsPeriod());
        }

        [Fact]
        public void RetrieveLocaleIP_Ok()
        {
            var ipSvr = DictHelper.RetrieveLocaleIPSvr();
            Assert.Equal("None", ipSvr);

            var ipUri = DictHelper.RetrieveLocaleIPSvrUrl("JuheIPSvr");
            Assert.NotNull(ipUri);
        }

        [Fact]
        public async void BaiduIPSvr_Ok()
        {
            var ipUri = DictHelper.RetrieveLocaleIPSvrUrl("BaiDuIPSvr");

            var client = HttpClientFactory.Create();

            // 日本东京
            var locator = await client.GetAsJsonAsync<BaiDuIPLocator>($"{ipUri}207.148.111.94");
            Assert.NotEqual("0", locator.Status);

            // 四川成都
            locator = await client.GetAsJsonAsync<BaiDuIPLocator>($"{ipUri}182.148.123.196");
            Assert.Equal("0", locator.Status);
        }

        [Fact]
        public async void JuheIPSvr_Ok()
        {
            var ipUri = DictHelper.RetrieveLocaleIPSvrUrl("JuheIPSvr");

            // 日本东京
            var client = HttpClientFactory.Create();
            var locator = await client.GetAsJsonAsync<JuheIPLocator>($"{ipUri}207.148.111.94");
            Assert.Contains(new int[] { 0, 10012 }, c => c == locator.Error_Code);

            // 四川成都
            locator = await client.GetAsJsonAsync<JuheIPLocator>($"{ipUri}182.148.123.196");
            Assert.Contains(new int[] { 0, 10012 }, c => c == locator.Error_Code);
        }

        [Fact]
        public void RetrieveAccessLogPeriod_Ok()
        {
            Assert.Equal(1, DictHelper.RetrieveAccessLogPeriod());
        }

        [Fact]
        public void IPSvrCachePeriod_Ok()
        {
            Assert.Equal("10", DictHelper.RetrieveLocaleIPSvrCachePeriod());
        }

        [Fact]
        public void DatabaseCheck_Ok()
        {
            var dict = new BootstrapDict() { Category = "系统检查", Name = "系统设置", Code = DatabaseName, Define = 0 };
            Assert.True(DictHelper.Save(dict));
            Assert.Equal(DatabaseName, DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == dict.Category && d.Name == dict.Name)?.Code ?? "unknown");
            Assert.True(DictHelper.Delete(new string[] { dict.Id }));
        }

        #region Private Class For Test
        /// <summary>
        /// 
        /// </summary>
        private class BaiDuIPLocator
        {
            /// <summary>
            /// 详细地址信息
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// 结果状态返回码
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Status == "0" ? string.Join(" ", Address.SpanSplit("|").Skip(1).Take(2)) : "XX XX";
            }
        }

        private class JuheIPLocator
        {
            /// <summary>
            /// 
            /// </summary>
            public string ResultCode { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Reason { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public JuheIPLocatorResult Result { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public int Error_Code { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Error_Code != 0 ? "XX XX" : Result.ToString();
            }
        }

        private class JuheIPLocatorResult
        {
            /// <summary>
            /// 
            /// </summary>
            public string Country { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Province { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Isp { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Country != "中国" ? $"{Country} {Province} {Isp}" : $"{Province} {City} {Isp}";
            }
        }
        #endregion
    }
}
