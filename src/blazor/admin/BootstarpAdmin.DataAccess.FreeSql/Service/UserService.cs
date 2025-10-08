// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;

namespace BootstrapAdmin.DataAccess.FreeSql.Service;

class UserService(IFreeSql freeSql) : IUser
{
    public bool Authenticate(string userName, string password)
    {
        var user = freeSql.Select<User>().Where(s => s.ApprovedTime != null && s.UserName == userName).ToOne(s => new User
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
        return freeSql.Select<User>().ToList();
    }

    public List<string> GetApps(string userName)
    {
        return freeSql.Ado.Query<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName) r on ra.RoleId = r.ID union select Code from Dicts where Category = @Category and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName)", new { UserName = userName, Category = "应用程序", RoleName = "Administrators" }).ToList();
    }

    /// <summary>
    /// 通过用户名获得指定的前台 AppId
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetAppIdByUserName(string userName) => freeSql.Select<User>().Where(s => s.UserName == userName).ToOne(s => s.App);

    public string? GetDisplayName(string? userName)
    {
        return freeSql.Select<User>().Where(s => s.UserName == userName).ToOne(s => s.DisplayName);
    }

    public List<string> GetRoles(string userName)
    {
        return freeSql.Ado.Query<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @userName union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName = @userName", new { userName }).ToList();
    }

    public User? GetUserByUserName(string? userName) => string.IsNullOrEmpty(userName) ? null : freeSql.Select<User>().Where(i => i.UserName == userName).ToOne();

    public List<string> GetUsersByGroupId(string? groupId)
    {
        return freeSql.Ado.Query<string>("select UserID from UserGroup where GroupID = @groupId", new { groupId }).ToList();
    }

    public List<string> GetUsersByRoleId(string? roleId)
    {
        return freeSql.Ado.Query<string>("select UserID from UserRole where RoleID = @roleId", new { roleId }).ToList();
    }

    public bool SaveUser(string userName, string displayName, string password)
    {
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        var user = freeSql.Select<User>().Where(s => s.UserName == userName).ToOne();
        bool ret = default;
        if (user == null)
        {
            // 开始事务
            freeSql.Transaction(() =>
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
                freeSql.Insert(user).ExecuteAffrows();
                // 授权 Default 角色
                freeSql.Ado.ExecuteNonQuery("insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = @userName", new { userName });
                ret = true;
            });
        }
        else
        {
            user.DisplayName = displayName;
            user.PassSalt = salt;
            user.Password = pwd;
            freeSql.Update<User>(user);
            ret = true;
        }
        return ret;
    }

    public bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds)
    {
        var ret = false;
        try
        {
            freeSql.Transaction(() =>
            {
                freeSql.Ado.ExecuteNonQuery("delete from UserGroup where GroupId = @groupId", new { groupId });
                freeSql.Insert(userIds.Select(g => new UserGroup { UserID = g, GroupID = groupId })).ExecuteAffrows();
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
            freeSql.Transaction(() =>
            {
                freeSql.Ado.ExecuteNonQuery("delete from UserRole where RoleID = @roleId", new { roleId });
                freeSql.Insert(userIds.Select(g => new UserRole { UserID = g, RoleID = roleId })).ExecuteAffrows();
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
            var user = freeSql.Select<User>().Where(s => s.UserName == phone).ToOne();
            if (user == null)
            {
                freeSql.Transaction(() =>
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
                    freeSql.Insert(user).ExecuteAffrows();
                    // Authorization
                    var roleIds = freeSql.Ado.Query<string>("select ID from Roles where RoleName in (@roles)", new { roles });
                    freeSql.Insert(roleIds.Select(g => new UserRole { RoleID = g, UserID = user.Id }));
                });
            }
            else
            {
                user.PassSalt = salt;
                user.Password = pwd;
                freeSql.Update<User>(user).ExecuteAffrows();
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
        var ret = freeSql.Update<User>().Set(t => t.App == app).Where(t => t.UserName == userName).ExecuteAffrows() > 0;
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
            ret = freeSql.Update<User>()
                .Set(t => t.Password == password)
                .Set(t => t.PassSalt == passSalt)
                .Where(t => t.UserName == userName)
                .ExecuteAffrows() > 0;
        }
        return ret;
    }

    public bool SaveDisplayName(string userName, string displayName)
    {
        var ret = freeSql.Update<User>()
            .Set(t => t.DisplayName == displayName)
            .Where(t => t.UserName == userName)
            .ExecuteAffrows() > 0;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveTheme(string userName, string theme)
    {
        var ret = freeSql.Update<User>()
            .Set(t => t.Css == theme)
            .Where(t => t.UserName == userName)
            .ExecuteAffrows() > 0;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveLogo(string userName, string? logo)
    {
        var ret = freeSql.Update<User>()
            .Set(t => t.Icon == logo)
            .Where(t => t.UserName == userName)
            .ExecuteAffrows() > 0;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
