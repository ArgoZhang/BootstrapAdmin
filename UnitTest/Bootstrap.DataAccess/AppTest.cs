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
            var rid = RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            AppHelper.SaveByRoleId(rid, new string[0]);
            Assert.NotEmpty(AppHelper.RetrievesByRoleId(rid));
        }

        [Fact]
        public void RetrievesByUserName_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            var userId = UserHelper.Retrieves().FirstOrDefault(u => u.UserName == "Admin").Id;
            UserHelper.SaveByRoleId(roleId, new string[] { userId });
            Assert.NotEmpty(AppHelper.RetrievesByUserName("Admin"));
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            var rid = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            Assert.True(AppHelper.SaveByRoleId(rid, new string[] { "2" }));
            Assert.NotEmpty(AppHelper.RetrievesByRoleId(rid).Where(r => r.Checked == "checked"));
        }
    }
}
