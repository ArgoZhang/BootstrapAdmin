﻿using System;
using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess.SqlServer
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

            var apps = AppHelper.RetrievesByRoleId(roleId);
            roleId = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Default").Id;
            AppHelper.SaveByRoleId(roleId, apps.Select(a => a.Id));
            Assert.NotEmpty(AppHelper.RetrievesByUserName("User"));
        }

        [Fact]
        public void SaveByRoleId_Ok()
        {
            var rid = RoleHelper.Retrieves().FirstOrDefault(r => r.RoleName == "Administrators").Id;
            Assert.True(AppHelper.SaveByRoleId(rid, null));
            Assert.True(AppHelper.SaveByRoleId(rid, new string[] { "2" }));
            Assert.NotEmpty(AppHelper.RetrievesByRoleId(rid).Where(r => r.Checked == "checked"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveByRoleId_Exception(string roleId)
        {
            Assert.ThrowsAny<ArgumentNullException>(() => AppHelper.SaveByRoleId(roleId, null));
        }
    }
}
