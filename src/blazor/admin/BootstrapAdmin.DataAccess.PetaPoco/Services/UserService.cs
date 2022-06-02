// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class UserService : IUser
{
    private IDBManager DBManager { get; }

    public UserService(IDBManager db)
    {
        DBManager = db;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<User> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<User>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Authenticate(string userName, string password)
    {
        using var db = DBManager.Create();
        var user = db.SingleOrDefault<User>("select DisplayName, Password, PassSalt from Users where ApprovedTime is not null and UserName = @0", userName);

        var isAuth = false;
        if (user != null && !string.IsNullOrEmpty(user.PassSalt))
        {
            isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }
        return isAuth;
    }

    private const string UserServiceGetUserByUserNameCacheKey = "UserService-GetUserByUserName";

    public User? GetUserByUserName(string? userName) => CacheManager.GetOrAdd($"{UserServiceGetUserByUserNameCacheKey}-{userName}", entry =>
    {
        using var db = DBManager.Create();
        return string.IsNullOrEmpty(userName) ? null : db.FirstOrDefault<User>("Where UserName = @0", userName);
    });

    private const string UserServiceGetAppsByUserNameCacheKey = "UserService-GetAppsByUserName";

    public List<string> GetApps(string userName) => CacheManager.GetOrAdd($"{UserServiceGetAppsByUserNameCacheKey}-{userName}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0) r on ra.RoleId = r.ID union select Code from Dicts where Category = @1 and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 and r.RoleName = @2 union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0 and r.RoleName = @2)", userName, "应用程序", "Administrators");
    });

    private const string UserServiceGetAppIdByUserNameCacheKey = "UserService-GetAppIdByUserName";

    /// <summary>
    /// 通过用户名获得指定的前台 AppId
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetAppIdByUserName(string userName) => CacheManager.GetOrAdd($"{UserServiceGetAppIdByUserNameCacheKey}-{userName}", entry =>
    {
        using var db = DBManager.Create();
        return db.FirstOrDefault<User>("Where UserName = @0", userName)?.App;
    });

    private const string UserServiceGetRolesByUserNameCacheKey = "UserService-GetRolesByUserName";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<string> GetRoles(string userName) => CacheManager.GetOrAdd($"{UserServiceGetRolesByUserNameCacheKey}-{userName}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @0 union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName = @0", userName);
    });

    private const string UserServiceGetUsersByGroupIdCacheKey = "UserService-GetUsersByGroupId";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    public List<string> GetUsersByGroupId(string? groupId) => CacheManager.GetOrAdd($"{UserServiceGetUsersByGroupIdCacheKey}-{groupId}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select UserID from UserGroup where GroupID = @0", groupId);
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveUsersByGroupId(string? id, IEnumerable<string> userIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from UserGroup where GroupId = @0", id);
            db.InsertBatch("UserGroup", userIds.Select(g => new { UserID = g, GroupID = id }));
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    private const string UserServiceGetUsersByRoleIdCacheKey = "UserService-GetUsersByRoleId";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetUsersByRoleId(string? roleId) => CacheManager.GetOrAdd($"{UserServiceGetUsersByRoleIdCacheKey}-{roleId}", entry =>
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select UserID from UserRole where RoleID = @0", roleId);
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from UserRole where RoleID = @0", roleId);
            db.InsertBatch("UserRole", userIds.Select(g => new { UserID = g, RoleID = roleId }));
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 更新密码方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    public bool ChangePassword(string userName, string password, string newPassword)
    {
        var ret = false;
        using var db = DBManager.Create();
        if (Authenticate(userName, password))
        {
            var passSalt = LgbCryptography.GenerateSalt();
            password = LgbCryptography.ComputeHash(newPassword, passSalt);
            string sql = "set Password = @0, PassSalt = @1 where UserName = @2";
            ret = db.Update<User>(sql, password, passSalt, userName) == 1;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool SaveDisplayName(string userName, string displayName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("set DisplayName = @1 where UserName = @0", userName, displayName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool SaveTheme(string userName, string theme)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("set Css = @1 where UserName = @0", userName, theme) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool SaveLogo(string userName, string? logo)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("set Icon = @1 where UserName = @0", userName, logo) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 创建手机用户
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            var salt = LgbCryptography.GenerateSalt();
            var pwd = LgbCryptography.ComputeHash(code, salt);
            var user = db.FirstOrDefault<User>("Where UserName = @0", phone);
            if (user == null)
            {
                db.BeginTransaction();
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
                db.Save(user);
                // Authorization
                var roleIds = db.Fetch<string>("select ID from Roles where RoleName in (@roles)", new { roles });
                db.InsertBatch("UserRole", roleIds.Select(g => new { RoleID = g, UserID = user.Id }));
                db.CompleteTransaction();
            }
            else
            {
                user.PassSalt = salt;
                user.Password = pwd;
                db.Update(user);
            }
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveUser(string userName, string displayName, string password)
    {
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        using var db = DBManager.Create();
        var user = db.FirstOrDefault<User>("Where UserName = @0", userName);
        bool ret;
        if (user == null)
        {
            try
            {
                // 开始事务
                db.BeginTransaction();
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
                db.Save(user);
                // 授权 Default 角色
                db.Execute("insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = @0", userName);
                // 结束事务
                db.CompleteTransaction();
                ret = true;
            }
            catch (Exception)
            {
                db.AbortTransaction();
                throw;
            }
        }
        else
        {
            user.DisplayName = displayName;
            user.PassSalt = salt;
            user.Password = pwd;
            db.Update(user);
            ret = true;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveApp(string userName, string app)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("Set App = @1 Where UserName = @0", userName, app) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
