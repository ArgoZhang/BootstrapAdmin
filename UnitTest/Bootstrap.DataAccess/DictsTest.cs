using System.Linq;
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
            Assert.Equal("~/Home/Index", dict.RetrieveHomeUrl());
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
    }
}
