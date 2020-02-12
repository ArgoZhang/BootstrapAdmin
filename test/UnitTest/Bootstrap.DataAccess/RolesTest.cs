using Bootstrap.Security.Mvc;
using System;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class RolesTest
    {
        [Fact]
        public void SaveRolesByUserId_Ok()
        {
            var userId = UserHelper.Retrieves().Where(u => u.UserName == "Admin").FirstOrDefault().Id;
            Assert.True(RoleHelper.SaveByUserId(userId, RoleHelper.Retrieves().Select(r => r.Id)));
        }

        [Fact]
        public void RetrieveRolesByUserId_Ok()
        {
            var userId = UserHelper.Retrieves().Where(u => u.UserName == "Admin").FirstOrDefault().Id;
            Assert.NotEmpty(RoleHelper.RetrievesByUserId(userId));
        }

        [Fact]
        public void DeleteRole_Ok()
        {
            var role = new Role()
            {
                Description = "Role_Desc",
                RoleName = "UnitTest-Delete"
            };
            Assert.True(RoleHelper.Save(role));
            Assert.True(RoleHelper.Delete(RoleHelper.Retrieves().Where(r => r.RoleName == role.RoleName).Select(r => r.Id)));
        }

        [Fact]
        public void SaveRole_Ok()
        {
            var role = new Role()
            {
                Description = "Role_Desc",
                RoleName = "UnitTest-Save"
            };

            // insert 
            Assert.True(RoleHelper.Save(role));

            // update
            Assert.True(RoleHelper.Save(role));

            // delete 
            Assert.True(RoleHelper.Delete(new string[] { role.Id }));
        }

        [Fact]
        public void RetrieveRolesByMenuId_Ok()
        {
            var roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            MenuHelper.SaveMenusByRoleId(roleId, MenuHelper.RetrieveAllMenus("Admin").Select(m => m.Id));
            var rs = RoleHelper.RetrievesByMenuId(MenuHelper.RetrieveAllMenus("Admin").FirstOrDefault().Id).Where(r => r.Checked == "checked");
            Assert.NotEmpty(rs);
        }

        [Fact]
        public void SavaRolesByMenuId_Ok()
        {
            var menuId = MenuHelper.RetrieveAllMenus("Admin").FirstOrDefault().Id;
            Assert.True(RoleHelper.SavaByMenuId(menuId, RoleHelper.Retrieves().Select(r => r.Id)));
        }

        [Fact]
        public void RetrieveRolesByGroupId_Ok()
        {
            var id = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            GroupHelper.SaveByRoleId(id, GroupHelper.Retrieves().Select(g => g.Id));
            Assert.NotEmpty(RoleHelper.RetrievesByGroupId(GroupHelper.Retrieves().Where(g => g.GroupName == "Admin").FirstOrDefault().Id).Where(r => r.Checked == "checked"));
        }

        [Fact]
        public void RetrieveRolesByUserName_Ok()
        {
            var id = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            UserHelper.SaveByRoleId(id, UserHelper.Retrieves().Select(u => u.Id));
            Assert.NotEmpty(RoleHelper.RetrievesByUserName("Admin"));

            // 新建用户 默认角色为 Default
            var user = new User { UserName = "UserForRoleTest", Password = "123", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "" };
            Assert.True(UserHelper.Save(user));
            Assert.Single(RoleHelper.RetrievesByUserName(user.UserName));
            Assert.True(UserHelper.Delete(UserHelper.Retrieves().Where(usr => usr.UserName == user.UserName).Select(usr => usr.Id)));
        }

        [Fact]
        public void RetrieveRolesByUrl_Ok()
        {
            Assert.NotEmpty(RoleHelper.RetrievesByUrl("~/Home/Index", BootstrapAppContext.AppId));
        }

        [Fact]
        public void SaveByGroupId_Ok()
        {
            var gId = GroupHelper.Retrieves().FirstOrDefault(g => g.GroupName == "Admin").Id;
            RoleHelper.SaveByGroupId(gId, RoleHelper.Retrieves().Where(r => r.RoleName == "Administrators").Select(r => r.Id));
        }
    }
}
