// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootStarpAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

class UserService : IUser
{
    private IFreeSql FreeSql { get; }

    public UserService(IFreeSql freeSql) => FreeSql = freeSql;

    public bool Authenticate(string userName, string password)
    {
        var user = FreeSql.Select<User>().Where(s => s.ApprovedTime != null && s.UserName == userName).ToOne(s => new User
        {
            DisplayName = s.DisplayName,
            PassSalt = s.PassSalt,
            Password = s.Password
        });

        var isAuth = false;
        if (user != null && !string.IsNullOrEmpty(user.PassSalt))
        {
            isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }
        return isAuth;
    }

    public List<User> GetAll()
    {
        return FreeSql.Select<User>().ToList();
    }

    private const string UserServiceGetAppsByUserNameCacheKey = "UserService-GetAppsByUserName";

    public List<string> GetApps(string userName) => CacheManager.GetOrAdd($"{UserServiceGetAppsByUserNameCacheKey}-{userName}", entry => FreeSql.Ado.Query<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName) r on ra.RoleId = r.ID union select Code from Dicts where Category = @Category and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName)", new { UserName = userName, Category = "应用程序", RoleName = "Administrators" }).ToList());

    /// <summary>
    /// 通过用户名获得指定的前台 AppId
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetAppIdByUserName(string userName) => FreeSql.Select<User>().Where(s => s.UserName == userName).ToOne(s => s.App);

    public string? GetDisplayName(string? userName)
    {
        return FreeSql.Select<User>().Where(s => s.UserName == userName).ToOne(s => s.DisplayName);
    }

    private const string UserServiceGetRolesByUserNameCacheKey = "UserService-GetRolesByUserName";

    public List<string> GetRoles(string userName) => CacheManager.GetOrAdd($"{UserServiceGetRolesByUserNameCacheKey}-{userName}", entry => FreeSql.Ado.Query<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @userName union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName = @userName", new { userName }).ToList());

    private const string UserServiceGetUserByUserNameCacheKey = "UserService-GetUserByUserName";

    public User? GetUserByUserName(string? userName) => CacheManager.GetOrAdd($"{UserServiceGetUserByUserNameCacheKey}-{userName}", entry => string.IsNullOrEmpty(userName) ? null : FreeSql.Select<User>().Where(i => i.UserName == userName).ToOne());

    private const string UserServiceGetUsersByGroupIdCacheKey = "UserService-GetUsersByGroupId";

    public List<string> GetUsersByGroupId(string? groupId) => CacheManager.GetOrAdd($"{UserServiceGetUsersByGroupIdCacheKey}-{groupId}", entry => FreeSql.Ado.Query<string>("select UserID from UserGroup where GroupID = @groupId", new { groupId }).ToList());

    private const string UserServiceGetUsersByRoleIdCacheKey = "UserService-GetUsersByRoleId";

    public List<string> GetUsersByRoleId(string? roleId) => CacheManager.GetOrAdd($"{UserServiceGetUsersByRoleIdCacheKey}-{roleId}", entry => FreeSql.Ado.Query<string>("select UserID from UserRole where RoleID = @roleId", new { roleId }).ToList());

    public bool SaveUser(string userName, string displayName, string password)
    {
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        var user = FreeSql.Select<User>().Where(s => s.UserName == userName).ToOne();
        bool ret = default;
        if (user == null)
        {
            // 开始事务
            FreeSql.Transaction(() =>
            {
                user = new User()
                {
                    Id = "0",
                    ApprovedBy = "System",
                    ApprovedTime = DateTime.Now,
                    DisplayName = "手机用户",
                    UserName = userName,
                    Icon = "default.jpg",
                    Description = "系统默认创建",
                    PassSalt = salt,
                    Password = pwd
                };
                FreeSql.Insert(user).ExecuteAffrows();
                // 授权 Default 角色
                FreeSql.Ado.ExecuteNonQuery("insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = @userName", new { userName });
                ret = true;
            });
        }
        else
        {
            user.DisplayName = displayName;
            user.PassSalt = salt;
            user.Password = pwd;
            FreeSql.Update<User>(user);
            ret = true;
        }
        return ret;
    }

    public bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from UserGroup where GroupId = @groupId", new { groupId });
                FreeSql.Insert(userIds.Select(g => new UserGroup { UserID = g, GroupID = groupId })).ExecuteAffrows();
            });

            ret = true;
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from UserRole where RoleID = @roleId", new { roleId });
                FreeSql.Insert(userIds.Select(g => new UserRole { UserID = g, RoleID = roleId })).ExecuteAffrows();
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles)
    {
        var ret = false;
        try
        {
            var salt = LgbCryptography.GenerateSalt();
            var pwd = LgbCryptography.ComputeHash(code, salt);
            var user = FreeSql.Select<User>().Where(s => s.UserName == phone).ToOne();
            if (user == null)
            {
                FreeSql.Transaction(() =>
                {
                    user = new User()
                    {
                        ApprovedBy = "Mobile",
                        ApprovedTime = DateTime.Now,
                        DisplayName = "手机用户",
                        UserName = phone,
                        Icon = "default.jpg",
                        Description = "手机用户",
                        PassSalt = salt,
                        Password = LgbCryptography.ComputeHash(code, salt),
                        App = appId
                    };
                    FreeSql.Insert(user).ExecuteAffrows();
                    // Authorization
                    var roleIds = FreeSql.Ado.Query<string>("select ID from Roles where RoleName in (@roles)", new { roles });
                    FreeSql.Insert(roleIds.Select(g => new UserRole { RoleID = g, UserID = user.Id })).ExecuteAffrows();
                });
            }
            else
            {
                user.PassSalt = salt;
                user.Password = pwd;
                FreeSql.Update<User>(user).ExecuteAffrows();
            }
            ret = true;
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    public bool SaveApp(string userName, string app)
    {
        var ret = FreeSql.Ado.ExecuteNonQuery("update users  set App = @App Where UserName = @UserName", new { App = app, UserName = userName }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool ChangePassword(string userName, string password, string newPassword)
    {
        var ret = false;
        if (Authenticate(userName, password))
        {
            var passSalt = LgbCryptography.GenerateSalt();
            password = LgbCryptography.ComputeHash(newPassword, passSalt);
            string sql = "update users set Password = @Password, PassSalt = @PassSalt where UserName = @UserName";
            ret = FreeSql.Ado.ExecuteNonQuery(sql, new { Password = password, PassSalt = passSalt, UserName = userName }) == 1;
        }
        return ret;
    }

    public bool SaveDisplayName(string userName, string displayName)
    {
        var ret = FreeSql.Ado.ExecuteNonQuery("update users set DisplayName = @DisplayName where UserName = @UserName", new { DisplayName = displayName, UserName = userName }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveTheme(string userName, string theme)
    {
        var ret = FreeSql.Ado.ExecuteNonQuery("update users set Css = @Css where UserName = @UserName", new { Css = theme, UserName = userName }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveLogo(string userName, string? logo)
    {
        var ret = FreeSql.Ado.ExecuteNonQuery("update users set Icon = @Icon where UserName = @UserName", new { Icon = logo, UserName = userName }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
