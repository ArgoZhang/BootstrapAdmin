// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootStarpAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using SqlSugar;

namespace BootStarpAdmin.DataAccess.SqlSugar.Service;

/// <summary>
/// 
/// </summary>
public class UserService : IUser
{
    private ISqlSugarClient Client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    public UserService(ISqlSugarClient client) => Client = client;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<User> GetAll()
    {
        return Client.Queryable<User>().IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool Authenticate(string userName, string password)
    {
        var user = Client.Ado.SqlQuery<User>("select DisplayName, Password, PassSalt from Users where ApprovedTime is not null and UserName = @UserName", new { UserName = userName }).First();

        var isAuth = false;
        if (user != null && !string.IsNullOrEmpty(user.PassSalt))
        {
            isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }
        return isAuth;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    public bool ChangePassword(string userName, string password, string newPassword)
    {
        var ret = false;
        if (Authenticate(userName, password))
        {
            var passSalt = LgbCryptography.GenerateSalt();
            password = LgbCryptography.ComputeHash(newPassword, passSalt);
            string sql = "update users set Password = @Password, PassSalt = @PassSalt where UserName = @UserName";
            ret = Client.Ado.ExecuteCommand(sql, new { Password = password, PassSalt = passSalt, UserName = userName }) == 1;
        }
        return ret;
    }

    private const string UserServiceGetAppIdByUserNameCacheKey = "UserService-GetAppIdByUserName";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetAppIdByUserName(string userName) => CacheManager.GetOrAdd($"{UserServiceGetAppIdByUserNameCacheKey}-{userName}", entry => Client.Queryable<User>().IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").Where(s => s.UserName == userName).First()?.App);

    private const string UserServiceGetAppsByUserNameCacheKey = "UserService-GetAppsByUserName";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetApps(string userName) => CacheManager.GetOrAdd($"{UserServiceGetAppsByUserNameCacheKey}-{userName}", entry => Client.Ado.SqlQuery<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName) r on ra.RoleId = r.ID union select Code from Dicts where Category = @Category and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName)", new { UserName = userName, Category = "应用程序", RoleName = "Administrators" }));

    private const string UserServiceGetRolesByUserNameCacheKey = "UserService-GetRolesByUserName";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetRoles(string userName) => CacheManager.GetOrAdd($"{UserServiceGetRolesByUserNameCacheKey}-{userName}", entry => Client.Ado.SqlQuery<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @UserName union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName=@UserName", new { UserName = userName }));

    private const string UserServiceGetUserByUserNameCacheKey = "UserService-GetUserByUserName";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public User? GetUserByUserName(string? userName) => CacheManager.GetOrAdd($"{UserServiceGetUserByUserNameCacheKey}-{userName}", entry => string.IsNullOrEmpty(userName) ? null : Client.Queryable<User>().IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").Where(s => s.UserName == userName).First());

    private const string UserServiceGetUsersByGroupIdCacheKey = "UserService-GetUsersByGroupId";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<string> GetUsersByGroupId(string? groupId) => CacheManager.GetOrAdd($"{UserServiceGetUsersByGroupIdCacheKey}-{groupId}", entry => Client.Ado.SqlQuery<string>("select UserID from UserGroup where GroupID = @GroupID", new { GroupID = groupId }));

    private const string UserServiceGetUsersByRoleIdCacheKey = "UserService-GetUsersByRoleId";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<string> GetUsersByRoleId(string? roleId) => CacheManager.GetOrAdd($"{UserServiceGetUsersByRoleIdCacheKey}-{roleId}", entry => Client.Ado.SqlQuery<string>("select UserID from UserRole where RoleID = @RoleID", new { RoleID = roleId }));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveApp(string userName, string app)
    {
        var ret = Client.Ado.ExecuteCommand("update users set App = @App Where UserName = @UserName", new { App = app, UserName = userName }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveDisplayName(string userName, string displayName)
    {
        var ret = Client.Ado.ExecuteCommand("update users set DisplayName = @DisplayName where UserName = @UserName", new { UserName = userName, DisplayName = displayName }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="logo"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveLogo(string userName, string? logo)
    {
        var ret = Client.Ado.ExecuteCommand("update users set Icon = @Icon where UserName = @UserName", new { UserName = userName, Icon = logo }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="theme"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    public bool SaveTheme(string userName, string theme)
    {
        var ret = Client.Ado.ExecuteCommand("update users set Css = @Css where UserName = @UserName", new { UserName = userName, Css = theme }) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveUser(string userName, string displayName, string password)
    {
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        var user = Client.Queryable<User>().IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").First(s => s.UserName == userName);
        bool ret;
        if (user == null)
        {
            try
            {
                // 开始事务
                Client.Ado.BeginTran();
                user = new User()
                {
                    ApprovedBy = "System",
                    ApprovedTime = DateTime.Now,
                    DisplayName = "手机用户",
                    UserName = userName,
                    Icon = "default.jpg",
                    Description = "系统默认创建",
                    PassSalt = salt,
                    Password = pwd
                };
                Client.Insertable(user).IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").ExecuteCommand();
                // 授权 Default 角色
                Client.Ado.ExecuteCommand("insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = @userName", new { UserName = userName });
                // 结束事务
                Client.Ado.CommitTran();
                ret = true;
            }
            catch (Exception)
            {
                Client.Ado.RollbackTran();
                throw;
            }
        }
        else
        {
            user.DisplayName = displayName;
            user.PassSalt = salt;
            user.Password = pwd;
            Client.Updateable(user).IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").ExecuteCommand();
            ret = true;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds)
    {
        var ret = false;
        try
        {
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from UserGroup where GroupId = @GroupId", new { GroupId = groupId });
            Client.Insertable<UserGroup>(userIds.Select(g => new { UserID = g, GroupID = groupId })).ExecuteCommand();
            Client.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            Client.Ado.RollbackTran();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        var ret = false;
        try
        {
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from UserRole where RoleID = @RoleID", new { RoleID = roleId });
            Client.Insertable<UserRole>(userIds.Select(g => new { UserID = g, RoleID = roleId })).ExecuteCommand();
            Client.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            Client.Ado.RollbackTran();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
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
    /// <exception cref="NotImplementedException"></exception>
    public bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles)
    {
        var ret = false;
        try
        {
            var salt = LgbCryptography.GenerateSalt();
            var pwd = LgbCryptography.ComputeHash(code, salt);
            var user = Client.Queryable<User>().IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").First(s => s.UserName == phone);
            if (user == null)
            {
                Client.Ado.BeginTran();
                // 插入用户
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
                Client.Insertable(user).IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").ExecuteCommand();
                // Authorization
                var roleIds = Client.Ado.SqlQuery<string>("select ID from Roles where RoleName in (@roles)", new { roles = roles });
                Client.Insertable<UserRole>(roleIds.Select(g => new { RoleID = g, UserID = user.Id })).ExecuteCommand();
                Client.Ado.CommitTran();
            }
            else
            {
                user.PassSalt = salt;
                user.Password = pwd;
                Client.Updateable(user).IgnoreColumns("NewPassword", "ConfirmPassword", "Period", "IsReset").AS("Users").ExecuteCommand();
            }
            ret = true;
        }
        catch (Exception)
        {
            Client.Ado.RollbackTran();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
