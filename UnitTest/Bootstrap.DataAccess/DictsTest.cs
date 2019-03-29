using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class DictsTest
    {
        [Fact]
        public void SaveAndDelete_Ok()
        {
            var dict = new Dict()
            {
                Category = "UnitTest",
                Name = "Test1",
                Code = "1",
                Define = 1
            };
            Assert.True(dict.Save(dict));
            Assert.True(dict.Delete(dict.RetrieveDicts().Where(d => d.Category == dict.Category).Select(d => d.Id)));
        }

        [Fact]
        public void SaveSettings_Ok()
        {
            var dict = new Dict()
            {
                Category = "UnitTest",
                Name = "Test1",
                Code = "1",
                Define = 1
            };
            Assert.True(dict.SaveSettings(dict));
            dict.Delete(dict.RetrieveDicts().Where(d => d.Category == dict.Category).Select(d => d.Id));
        }

        [Fact]
        public void RetrieveCategories_Ok()
        {
            var dict = new Dict();
            Assert.NotEmpty(dict.RetrieveCategories());
        }

        [Fact]
        public void RetrieveWebTitle_Ok()
        {
            var dict = new Dict();
            Assert.Equal("后台管理系统", dict.RetrieveWebTitle());
        }

        [Fact]
        public void RetrieveWebFooter_Ok()
        {
            var dict = new Dict();
            Assert.Equal("2016 © 通用后台管理系统", dict.RetrieveWebFooter());
        }

        [Fact]
        public void RetrieveThemes_Ok()
        {
            var dict = new Dict();
            Assert.NotEmpty(dict.RetrieveThemes());
        }

        [Fact]
        public void RetrieveActiveTheme_Ok()
        {
            var dict = new Dict();
            Assert.Equal("blue.css", dict.RetrieveActiveTheme());
        }

        [Fact]
        public void RetrieveIconFolderPath_Ok()
        {
            var dict = new Dict();
            Assert.Equal("~/images/uploader/", dict.RetrieveIconFolderPath());
        }

        [Fact]
        public void RetrieveHomeUrl_Ok()
        {
            var dict = new Dict();
            Assert.Equal("~/Home/Index", dict.RetrieveHomeUrl("0"));
        }

        [Fact]
        public void RetrieveApps_Ok()
        {
            var dict = new Dict();
            Assert.NotEmpty(dict.RetrieveApps());
        }

        [Fact]
        public void RetrieveDicts_Ok()
        {
            var dict = new Dict();
            Assert.NotEmpty(dict.RetrieveDicts());
        }

        [Fact]
        public void RetrieveCookieExpiresPeriod_Ok()
        {
            var dict = new Dict();
            Assert.Equal(7, dict.RetrieveCookieExpiresPeriod());
        }

        [Fact]
        public void RetrieveExceptionsLogPeriod_Ok()
        {
            var dict = new Dict();
            Assert.Equal(1, dict.RetrieveExceptionsLogPeriod());
        }

        [Fact]
        public void RetrieveLoginLogsPeriod_Ok()
        {
            var dict = new Dict();
            Assert.Equal(12, dict.RetrieveLoginLogsPeriod());
        }

        [Fact]
        public void RetrieveLogsPeriod_Ok()
        {
            var dict = new Dict();
            Assert.Equal(12, dict.RetrieveLogsPeriod());
        }

        [Fact]
        public void RetrieveLocaleIP_Ok()
        {
            var dict = new Dict();
            var ipSvr = dict.RetrieveLocaleIPSvr();
            Assert.Equal("JuheIPSvr", ipSvr);

            var ipUri = dict.RetrieveLocaleIPSvrUrl(ipSvr);
            Assert.NotNull(ipUri);
        }

        [Fact]
        public async void BaiduIPSvr_Ok()
        {
            var dict = new Dict();
            var ipUri = dict.RetrieveLocaleIPSvrUrl("BaiDuIPSvr");

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
            var dict = new Dict();
            var ipUri = dict.RetrieveLocaleIPSvrUrl("JuheIPSvr");

            // 日本东京
            var client = HttpClientFactory.Create();
            var locator = await client.GetAsJsonAsync<JuheIPLocator>($"{ipUri}207.148.111.94");
            Assert.Equal(0, locator.Error_Code);

            // 四川成都
            locator = await client.GetAsJsonAsync<JuheIPLocator>($"{ipUri}182.148.123.196");
            Assert.Equal(0, locator.Error_Code);
        }

        [Fact]
        public void RetrieveAccessLogPeriod_Ok()
        {
            var dict = new Dict();
            Assert.Equal(1, dict.RetrieveAccessLogPeriod());
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
