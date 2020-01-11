using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess.SQLite
{
    [Collection("SQLiteContext")]
    public class DictsTest : SqlServer.DictsTest
    {
        protected override string DatabaseName { get; set; } = "SQLite";

        [Fact]
        public void RetrieveHomeUrl_Ok()
        {
            Assert.Equal("~/Home/Index", DictHelper.RetrieveHomeUrl("Admin", ""));

            var dict = DictHelper.RetrieveDicts().FirstOrDefault(d => d.Category == "网站设置" && d.Name == "默认应用程序");
            Assert.NotNull(dict);
            dict.Code = "1";
            DictHelper.Save(dict);
            Assert.Equal("http://localhost:49185/", DictHelper.RetrieveHomeUrl("Admin", "BA"));
            dict.Code = "0";
            DictHelper.Save(dict);
            Assert.Equal("~/Home/Index", DictHelper.RetrieveHomeUrl("Admin", "BA"));
            Assert.Equal("http://localhost:49185/", DictHelper.RetrieveHomeUrl("Admin", "Demo"));
        }
    }
}
