using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class RolesTest
    {
        [Fact]
        public void SaveRolesByUserId_Ok()
        {
            var role = new Role();
            Assert.True(role.SaveByUserId("1", new string[] { "1", "2" }));
        }

        [Fact]
        public void RetrieveRolesByUserId_Ok()
        {
            var role = new Role();
            Assert.NotEmpty(role.RetrievesByUserId("1"));
        }

        [Fact]
        public void DeleteRole_Ok()
        {
            var role = new Role()
            {
                Description = "Role_Desc",
                RoleName = "UnitTest-Delete"
            };
            role.Save(role);
            Assert.True(role.Delete(new string[] { role.Id.ToString() }));

            // clean
            role.Delete(role.Retrieves().Where(r => r.RoleName == role.RoleName).Select(r => r.Id));
        }

        [Fact]
        public void SaveRole_Ok()
        {
            var role = new Role()
            {
                Description = "Role_Desc",
                RoleName = "UnitTest-Save"
            };
            Assert.True(role.Save(role));

            // clean
            role.Delete(role.Retrieves().Where(r => r.RoleName == role.RoleName).Select(r => r.Id));
        }

        [Fact]
        public void RetrieveRolesByMenuId_Ok()
        {
            var menu = new Menu();
            var role = new Role();
            var id = role.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            menu.SaveMenusByRoleId(id, new string[] { "1" });
            var rs = role.RetrievesByMenuId("1");
            Assert.Contains(rs, r => r.Checked == "checked");
        }

        [Fact]
        public void SavaRolesByMenuId_Ok()
        {
            var role = new Role();
            Assert.True(role.SavaByMenuId("1", new string[] { "1" }));
        }

        [Fact]
        public void RetrieveRolesByGroupId_Ok()
        {
            var role = new Role();
            var id = role.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            new Group().SaveByRoleId(id, new string[] { "1" });
            Assert.Contains(role.RetrievesByGroupId("1"), r => r.Checked == "checked");
        }

        [Fact]
        public void RetrieveRolesByUserName_Ok()
        {
            var role = new Role();
            var id = role.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            new User().SaveByRoleId(id, new string[] { "1" });
            Assert.NotEmpty(role.RetrieveRolesByUserName("Admin"));
        }

        [Fact]
        public void RetrieveRolesByUrl_Ok()
        {
            var role = new Role();
            Assert.NotEmpty(role.RetrieveRolesByUrl("~/Home/Index"));
        }
    }
}
