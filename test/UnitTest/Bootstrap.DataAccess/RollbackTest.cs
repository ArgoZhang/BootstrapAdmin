using Bootstrap.Admin;
using System;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("Login")]
    public class RollbackTest
    {
        [Fact]
        public void App_Save()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new App().SaveByRoleId("1", new string[] { "2" })));
        }

        [Fact]
        public void Group_Delete()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Group().Delete(new string[] { "0" })));
        }

        [Fact]
        public void Group_SaveByUser()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Group().SaveByUserId("1", new string[] { "1" })));
        }

        [Fact]
        public void Group_SaveByRole()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Group().SaveByRoleId("1", new string[] { "1" })));
        }

        [Fact]
        public void Menu_Delete()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Menu().Delete(new string[] { "0" })));
        }

        [Fact]
        public void Menu_Save()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Menu().SaveMenusByRoleId("1", new string[] { "1" })));
        }

        [Fact]
        public void Role_Delete()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Role().Delete(new string[] { "0" })));
        }

        [Fact]
        public void Role_SaveByMenu()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Role().SavaByMenuId("1", new string[] { "1" })));
        }

        [Fact]
        public void Role_SaveByUser()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Role().SaveByUserId("1", new string[] { "1" })));
        }

        [Fact]
        public void Role_SaveByGroup()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new Role().SaveByGroupId("1", new string[] { "1" })));
        }

        [Fact]
        public void User_Delete()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new User().Delete(new string[] { "0" })));
        }

        [Fact]
        public void User_Reset()
        {
            var newUser = new User() { UserName = "U_Reset", DisplayName = "UnitTest", ApprovedTime = DateTime.Now, ApprovedBy = "System", Password = "1", Description = "UnitTest", RegisterTime = DateTime.Now };
            Assert.True(UserHelper.Save(newUser));
            Assert.True(UserHelper.ForgotPassword(new ResetUser() { DisplayName = "UnitTest", Reason = "UnitTest", ResetTime = DateTime.Now, UserName = newUser.UserName }));
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokePocoMapper<User>(() => new User().ResetPassword(newUser.UserName, "123789")));
            Assert.True(UserHelper.Delete(new string[] { newUser.Id }));
        }

        [Fact]
        public void User_Rejet()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new User().Reject("0", "User")));
        }

        [Fact]
        public void User_Save()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new User().Save(new User() { Password = "1", UserName = "U_Save", DisplayName = "UnitTest" })));
        }

        [Fact]
        public void User_SaveByRole()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new User().SaveByRoleId("1", new string[] { "1" })));
        }

        [Fact]
        public void User_SaveByMenu()
        {
            Assert.ThrowsAny<Exception>(() => TestHelper.RevokeMapper(() => new User().SaveByGroupId("1", new string[] { "1" })));
        }

        [Fact]
        public void Exceptions_Log()
        {
            TestHelper.RevokePocoMapper<Exceptions>(() => new Exceptions().Log(new Exception(), null));
        }
    }
}
