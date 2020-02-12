using Bootstrap.Security;
using Xunit;

namespace Bootstrap.DataAccess.MongoDB
{
    [Collection("MongoContext")]
    public class MenusTest : DataAccess.MenusTest
    {
        [Fact]
        public void Save_EmptyId()
        {
            var poco = new BootstrapMenu()
            {
                Id = "",
                Name = "UnitTest",
                Application = "0",
                Category = "0",
                Icon = "fa fa-fa",
                IsResource = 0,
                Target = "_blank",
                Order = 10,
                Url = "#",
                ParentId = "0",
                ParentName = "Test"
            };

            // insert
            Assert.True(MenuHelper.Save(poco));

            // clean
            MenuHelper.Delete(new string[] { poco.Id });
        }
    }
}
