using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class AppTest
    {
        [Fact]
        public void RetrievesByRoleId_Ok()
        {
            var db = DbManager.Create();
            db.Execute("delete from RoleApp");
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var app = new App();
            Assert.NotEmpty(app.RetrievesByRoleId(rid));
        }

        [Fact]
        public void RetrievesByUserName_Ok()
        {
            var app = new App();
            Assert.NotEmpty(app.RetrievesByUserName("Admin"));
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            var db = DbManager.Create();
            db.Execute("delete from RoleApp");

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var app = new App();
            Assert.True(app.SaveByRoleId(rid, new string[] { "1", "2" }));

            var count = db.ExecuteScalar<int>("select count(Id) from RoleApp where RoleID = @0", rid);
            Assert.Equal(2, count);
        }
    }
}
