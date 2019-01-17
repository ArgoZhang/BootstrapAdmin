using Bootstrap.Admin.Models;
using Bootstrap.DataAccess;
using Bootstrap.Security;
using Longbow.Cache;
using Longbow.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;
using static Bootstrap.Admin.Controllers.Api.ExceptionsController;

namespace Bootstrap.Admin.Api
{
    public class ApiTest : ApiWebHost
    {
        private BAWebHost _factory;

        public ApiTest(BAWebHost factory) : base(factory, true)
        {
            _factory = factory;
        }

        [Fact]
        public async void Users_Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "Users");
            var resp = await Client.SendAsync(req);
        }

        [Fact]
        public async void Users_Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "Users?sort=DisplayName&order=asc&offset=0&limit=20&name=Admin&displayName=Administrator&_=1547628247338";
            var qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void Users_PostAndDelete_Ok()
        {
            var user = new User();
            user.Delete(user.Retrieves().Where(usr => usr.UserName == "UnitTest-Delete").Select(usr => usr.Id));

            var nusr = new User { UserName = "UnitTest-Delete", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            var resp = await Client.PostAsJsonAsync<User, bool>("Users", nusr);
            Assert.True(resp);

            nusr.Id = user.Retrieves().First(u => u.UserName == nusr.UserName).Id;
            resp = await Client.PostAsJsonAsync<User, bool>("Users", nusr);
            Assert.True(resp);

            var ids = user.Retrieves().Where(d => d.UserName == nusr.UserName).Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("Users", ids));
        }

        [Fact]
        public async void Users_PostById_Ok()
        {
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;

            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"Users/{rid}?type=role", string.Empty);
            Assert.NotNull(ret);

            var gid = new Group().Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"Users/{gid}?type=group", string.Empty);
            Assert.NotNull(ret);
        }

        [Fact]
        public async void Users_PutById_Ok()
        {
            var ids = new User().Retrieves().Where(u => u.UserName == "Admin").Select(u => u.Id);
            var gid = new Group().Retrieves().Where(r => r.GroupName == "Admin").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Users/{gid}?type=group", ids);
            Assert.True(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Users/{rid}?type=role", ids);
            Assert.True(ret);
        }

        [Fact]
        public async void Users_Put_Ok()
        {
            var usr = new User { UserName = "UnitTest-Change", Password = "1", DisplayName = "DisplayName", ApprovedBy = "System", ApprovedTime = DateTime.Now, Description = "Desc", Icon = "default.jpg" };
            usr.Delete(usr.Retrieves().Where(u => u.UserName == usr.UserName).Select(u => u.Id));
            Assert.True(usr.Save(usr));

            // change theme
            usr.UserStatus = UserStates.ChangeTheme;
            var resp = await Client.PutAsJsonAsync<User, bool>("Users", usr);
            Assert.False(resp);

            // Login as new user
            await _factory.LoginAsync(Client, "UnitTest-Change", "1");
            resp = await Client.PutAsJsonAsync<User, bool>("Users", usr);
            Assert.True(resp);

            // change password
            usr.UserStatus = UserStates.ChangePassword;
            usr.NewPassword = "1";
            usr.Password = "1";
            resp = await Client.PutAsJsonAsync<User, bool>("Users", usr);
            Assert.True(resp);

            // change displayname
            usr.UserStatus = UserStates.ChangeDisplayName;
            resp = await Client.PutAsJsonAsync<User, bool>("Users", usr);
            Assert.True(resp);

            // delete 
            usr.Delete(new string[] { usr.Id });

            await _factory.LoginAsync(Client);
        }


        [Fact]
        public async void Category_Get_Ok()
        {
            var cates = await Client.GetAsJsonAsync<IEnumerable<string>>("Category");
            Assert.NotEmpty(cates);
        }

        [Fact]
        public async void Dicts_Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "Dicts?sort=Category&order=asc&offset=0&limit=20&category=%E8%8F%9C%E5%8D%95&name=%E7%B3%BB%E7%BB%9F%E8%8F%9C%E5%8D%95&define=0&_=1547608210979";
            var qd = await Client.GetAsJsonAsync<QueryData<BootstrapDict>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void Dicts_PostAndDelete_Ok()
        {
            var dict = new Dict();
            dict.Delete(new Dict().RetrieveDicts().Where(d => d.Category == "UnitTest-Category").Select(d => d.Id));

            var ret = await Client.PostAsJsonAsync<BootstrapDict, bool>("Dicts", new BootstrapDict() { Name = "UnitTest-Dict", Category = "UnitTest-Category", Code = "0", Define = 0 });
            Assert.True(ret);

            var ids = dict.RetrieveDicts().Where(d => d.Name == "UnitTest-Dict").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("Dicts", ids));
        }

        [Fact]
        public async void Exceptions_Get_Ok()
        {
            // insert exception
            var excep = new Exceptions();
            Assert.True(excep.Log(new Exception("UnitTest"), null));

            // 菜单 系统菜单 系统使用条件
            var query = "Exceptions?sort=LogTime&order=desc&offset=0&limit=20&StartTime=&EndTime=&_=1547610349796";
            var qd = await Client.GetAsJsonAsync<QueryData<BootstrapDict>>(query);
            Assert.NotEmpty(qd.rows);

            // clean
            DbManager.Create().Execute("delete from exceptions where AppDomainName = @0", AppDomain.CurrentDomain.FriendlyName);
        }

        [Fact]
        public async void Exceptions_Post_Ok()
        {
            var files = await Client.PostAsJsonAsync<string, IEnumerable<string>>("Exceptions", string.Empty);
            Assert.NotNull(files);

            var fileName = files.FirstOrDefault();
            if (!string.IsNullOrEmpty(fileName))
            {
                var resp = await Client.PutAsJsonAsync<ExceptionFileQuery, string>("Exceptions", new ExceptionFileQuery() { FileName = fileName });
                Assert.NotNull(resp);
            }

            // clean
            DbManager.Create().Execute("delete from exceptions where AppDomainName = @0", AppDomain.CurrentDomain.FriendlyName);
        }

        [Fact]
        public async void Groups_Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "Groups?sort=GroupName&order=asc&offset=0&limit=20&groupName=Admin&description=%E7%B3%BB%E7%BB%9F%E9%BB%98%E8%AE%A4%E7%BB%84&_=1547614230481";
            var qd = await Client.GetAsJsonAsync<QueryData<Group>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void Groups_GetById_Ok()
        {
            var id = new Group().Retrieves().Where(gp => gp.GroupName == "Admin").First().Id;
            var g = await Client.GetAsJsonAsync<Group>($"Groups/{id}");
            Assert.Equal("Admin", g.GroupName);
        }

        [Fact]
        public async void Groups_PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<Group, bool>("Groups", new Group() { GroupName = "UnitTest-Group", Description = "UnitTest-Desc" });
            Assert.True(ret);

            var ids = new Group().Retrieves().Where(d => d.GroupName == "UnitTest-Group").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("Groups", ids));
        }

        [Fact]
        public async void Groups_PostById_Ok()
        {
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<Group>>($"Groups/{uid}?type=user", string.Empty);
            Assert.NotEmpty(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<Group>>($"Groups/{rid}?type=role", string.Empty);
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Groups_PutById_Ok()
        {
            var ids = new Group().Retrieves().Select(g => g.Id);
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Groups/{uid}?type=user", ids);
            Assert.True(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Groups/{rid}?type=role", ids);
            Assert.True(ret);
        }

        [Fact]
        public async void Interface_RetrieveDicts_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<BootstrapDict>>("Interface/RetrieveDicts", "");
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Interface_RetrieveRolesByUrl_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<string>>("Interface/RetrieveRolesByUrl", "~/Admin/Index");
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Interface_RetrieveRolesByUserName_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<string>>("Interface/RetrieveRolesByUserName", "Admin");
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Interface_RetrieveUserByUserName_Ok()
        {
            var ret = await Client.PostAsJsonAsync<string, BootstrapUser>("Interface/RetrieveUserByUserName", "Admin");
            Assert.Equal("Admin", ret.UserName);
        }

        [Fact]
        public async void Interface_RetrieveAppMenus_Ok()
        {
            var ret = await Client.PostAsJsonAsync<AppMenuOption, IEnumerable<BootstrapMenu>>("Interface/RetrieveAppMenus", new AppMenuOption() { AppId = "0", UserName = "Admin", Url = "~/Admin/Index" });
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Login_Login_Ok()
        {
            var resq = await Client.PostAsJsonAsync("Login", new { userName = "Admin", password = "123789" });
            var _token = await resq.Content.ReadAsStringAsync();
            Assert.NotNull(_token);
        }

        [Fact]
        public async void Login_Option_Ok()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "Login");
            var resp = await Client.SendAsync(req);
        }
        [Fact]
        public async void Logs_Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "Logs?sort=LogTime&order=desc&offset=0&limit=20&operateType=&OperateTimeStart=&OperateTimeEnd=&_=1547617573596";
            var qd = await Client.GetAsJsonAsync<QueryData<Log>>(query);
            Assert.NotEmpty(qd.rows);
        }

        [Fact]
        public async void Logs_Post_Ok()
        {
            Client.DefaultRequestHeaders.Add("user-agent", "UnitTest");
            var resp = await Client.PostAsJsonAsync<Log, bool>("Logs", new Log() { CRUD = "UnitTest", RequestUrl = "~/UnitTest" });
            Assert.True(resp);

            // clean
            DbManager.Create().Execute("delete from Logs where CRUD = @0", "UnitTest");
        }

        [Fact]
        public async void Menus_Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "Menus?sort=Order&order=asc&offset=0&limit=20&parentName=&name=%E5%90%8E%E5%8F%B0%E7%AE%A1%E7%90%86&category=0&isresource=0&_=1547619684999";
            var qd = await Client.GetAsJsonAsync<QueryData<object>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void Menus_PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<BootstrapMenu, bool>("Menus", new BootstrapMenu() { Name = "UnitTest-Menu", Application = "0", Category = "0", ParentId = "0", Url = "#", Target = "_self", IsResource = 0 });
            Assert.True(ret);

            var menu = new Menu();
            var ids = menu.RetrieveAllMenus("Admin").Where(d => d.Name == "UnitTest-Menu").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("Menus", ids));
        }


        [Fact]
        public async void Menus_PostById_Ok()
        {
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"Menus/{uid}?type=user", string.Empty);
            Assert.NotEmpty(ret);

            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"Menus/{rid}?type=role", string.Empty);
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Menus_PutById_Ok()
        {
            var ids = new Menu().RetrieveAllMenus("Admin").Select(g => g.Id);
            var rid = new Role().Retrieves().Where(r => r.RoleName == "Administrators").First().Id;
            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Menus/{rid}", ids);
            Assert.True(ret);
        }

        [Theory]
        [InlineData("inbox")]
        [InlineData("sendmail")]
        [InlineData("mark")]
        [InlineData("trash")]
        public async void Messages_Get_Ok(string action)
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<Message>>($"Messages/{action}");
            Assert.NotNull(resp);
        }

        [Fact]
        public async void Messages_GetCount_Ok()
        {
            var resp = await Client.GetAsJsonAsync<MessageCountModel>("Messages");
            Assert.NotNull(resp);
        }

        [Fact]
        public async void New_Get_Ok()
        {
            var nusr = InsertNewUser();

            var resp = await Client.GetAsJsonAsync<IEnumerable<object>>("New");
            Assert.NotEmpty(resp);

            // 删除新用户
            nusr.Delete(nusr.Retrieves().Where(u => u.UserName == nusr.UserName).Select(u => u.Id));
        }

        [Fact]
        public async void New_Put_Ok()
        {
            DeleteUnitTestUser();
            var nusr = InsertNewUser();

            // Approve
            nusr.UserStatus = UserStates.ApproveUser;
            var resp = await Client.PutAsJsonAsync<User, bool>("New", nusr);
            Assert.True(resp);

            // 删除新用户
            nusr.Delete(new string[] { nusr.Id });

            // Reject
            nusr = InsertNewUser();
            nusr.UserStatus = UserStates.RejectUser;
            resp = await Client.PutAsJsonAsync<User, bool>("New", nusr);
            Assert.True(resp);

            // 删除新用户
            nusr.Delete(new string[] { nusr.Id });
        }

        private User InsertNewUser()
        {
            // 插入新用户
            var nusr = new User() { UserName = "UnitTest-Register", DisplayName = "UnitTest", Password = "1", Description = "UnitTest" };
            Assert.True(new User().Save(nusr));
            return nusr;
        }

        private void DeleteUnitTestUser()
        {
            var ids = new User().RetrieveNewUsers().Where(u => u.UserName == "UnitTest-Register").Select(u => u.Id);
            new User().Delete(ids);
        }

        [Fact]
        public async void Notifications_Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<object>("Notifications");
            Assert.NotNull(resp);
        }

        [Fact]
        public async void Register_Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<bool>("Register?userName=Admin");
            Assert.False(resp);
        }

        [Fact]
        public async void Register_Post_Ok()
        {
            // register new user
            var nusr = new User() { UserName = "UnitTest-RegisterController", DisplayName = "UnitTest", Password = "1", Description = "UnitTest" };
            var resp = await Client.PostAsJsonAsync<User, bool>("Register", nusr);
            Assert.True(resp);

            nusr.Delete(nusr.RetrieveNewUsers().Where(u => u.UserName == nusr.UserName).Select(u => u.Id));
        }

        [Fact]
        public async void Roles_Get_Ok()
        {
            // 菜单 系统菜单 系统使用条件
            var query = "Roles?sort=RoleName&order=asc&offset=0&limit=20&roleName=Administrators&description=%E7%B3%BB%E7%BB%9F%E7%AE%A1%E7%90%86%E5%91%98&_=1547625202230";
            var qd = await Client.GetAsJsonAsync<QueryData<Group>>(query);
            Assert.Single(qd.rows);
        }

        [Fact]
        public async void Roles_PostAndDelete_Ok()
        {
            var ret = await Client.PostAsJsonAsync<Role, bool>("Roles", new Role() { RoleName = "UnitTest-Role", Description = "UnitTest-Desc" });
            Assert.True(ret);

            var ids = new Role().Retrieves().Where(d => d.RoleName == "UnitTest-Role").Select(d => d.Id);
            Assert.True(await Client.DeleteAsJsonAsync<IEnumerable<string>, bool>("Roles", ids));
        }

        [Fact]
        public async void Roles_PostById_Ok()
        {
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = new Group().Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = new Menu().RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;

            var ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"Roles/{uid}?type=user", string.Empty);
            Assert.NotEmpty(ret);

            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"Roles/{gid}?type=group", string.Empty);
            Assert.NotEmpty(ret);

            ret = await Client.PostAsJsonAsync<string, IEnumerable<object>>($"Roles/{mid}?type=menu", string.Empty);
            Assert.NotEmpty(ret);
        }

        [Fact]
        public async void Roles_PutById_Ok()
        {
            var uid = new User().Retrieves().Where(u => u.UserName == "Admin").First().Id;
            var gid = new Group().Retrieves().Where(g => g.GroupName == "Admin").First().Id;
            var mid = new Menu().RetrieveAllMenus("Admin").Where(m => m.Url == "~/Admin/Index").First().Id;
            var ids = new Role().Retrieves().Select(r => r.Id);

            var ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Roles/{uid}?type=user", ids);
            Assert.True(ret);

            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Roles/{gid}?type=group", ids);
            Assert.True(ret);

            ret = await Client.PutAsJsonAsync<IEnumerable<string>, bool>($"Roles/{mid}?type=menu", ids);
            Assert.True(ret);
        }

        [Fact]
        public async void Settings_Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<ICacheCorsItem>>("Settings");
            Assert.NotNull(resp);
        }

        [Fact]
        public async void Settings_Post_Ok()
        {
            var dict = new Dict();
            var dicts = dict.RetrieveDicts();

            var ids = dicts.Where(d => d.Category == "UnitTest-Settings").Select(d => d.Id);
            dict.Delete(ids);

            Assert.True(dict.Save(new Dict() { Category = "UnitTest-Settings", Name = "UnitTest", Code = "0", Define = 0 }));

            // 获得原来值
            var resp = await Client.PostAsJsonAsync<BootstrapDict, bool>("Settings", new Dict() { Category = "UnitTest-Settings", Name = "UnitTest", Code = "UnitTest" });
            Assert.True(resp);

            var code = dict.RetrieveDicts().FirstOrDefault(d => d.Category == "UnitTest-Settings").Code;
            Assert.Equal("UnitTest", code);

            // Delete 
            ids = dict.RetrieveDicts().Where(d => d.Category == "UnitTest-Settings").Select(d => d.Id);
            dict.Delete(ids);
        }

        [Fact]
        public async void Tasks_Get_Ok()
        {
            var resp = await Client.GetAsJsonAsync<IEnumerable<Task>>("Tasks");
            Assert.NotNull(resp);
        }
    }
}
