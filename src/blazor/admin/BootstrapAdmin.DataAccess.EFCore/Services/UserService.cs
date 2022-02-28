// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.EFCore.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

/// <summary>
/// 
/// </summary>
public class UserService : IUser
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factory"></param>
    public UserService(IDbContextFactory<BootstrapAdminContext> factory) => DbFactory = factory;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<User> GetAll()
    {
        using var context = DbFactory.CreateDbContext();
        return context.Users.AsNoTracking().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool Authenticate(string userName, string password)
    {
        var isAuth = false;
        using var context = DbFactory.CreateDbContext();
        var user = context.Users.Where(s => s.ApprovedTime != null).FirstOrDefault(x => x.UserName == userName);
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
    /// <returns></returns>
    public List<string> GetApps(string userName)
    {
        using var context = DbFactory.CreateDbContext();
        return context.Dicts.FromSqlRaw("select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = {0} union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = {0}) r on ra.RoleId = r.ID union select Code from Dicts where Category = {1} and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = {0} and r.RoleName = {2} union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = {0} and r.RoleName = {2})", new[] { userName, "应用程序", "Administrators" }).Select(s => s.Code).AsNoTracking().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetDisplayName(string? userName)
    {
        using var context = DbFactory.CreateDbContext();
        return string.IsNullOrEmpty(userName) ? "" : context.Users.FirstOrDefault(s => s.UserName == userName)?.DisplayName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetRoles(string userName)
    {
        using var context = DbFactory.CreateDbContext();
        return context.Roles.FromSqlRaw("select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = {0} union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName = {0}", userName).Select(s => s.RoleName).AsNoTracking().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public List<string> GetUsersByGroupId(string? groupId)
    {
        using var context = DbFactory.CreateDbContext();
        return context.UserGroup.Where(s => s.GroupId == groupId).Select(s => s.UserId!).AsNoTracking().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetUsersByRoleId(string? roleId)
    {
        using var context = DbFactory.CreateDbContext();
        return context.UserRole.Where(s => s.RoleId == roleId).Select(s => s.UserId!).AsNoTracking().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        dbcontext.Database.ExecuteSqlRaw("delete from UserGroup where GroupId = {0}", groupId!);
        dbcontext.AddRange(userIds.Select(g => new UserGroup { UserId = g, GroupId = groupId }));
        dbcontext.SaveChanges();
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        dbcontext.Database.ExecuteSqlRaw("delete from UserRole where RoleID = {0}", roleId!);
        dbcontext.AddRange(userIds.Select(g => new UserRole { UserId = g, RoleId = roleId }));
        dbcontext.SaveChanges();
        return true;
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
        using var dbcontext = DbFactory.CreateDbContext();
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(code, salt);
        var user = GetAll().FirstOrDefault(user => user.UserName == phone);
        if (user == null)
        {
            user = new User()
            {
                ApprovedBy = "Mobile",
                ApprovedTime = DateTime.Now,
                DisplayName = "手机用户",
                UserName = phone,
                Icon = "default.jpg",
                Description = "手机用户",
                App = appId
            };
            dbcontext.Add(user);

            // Authorization
            var roleIds = dbcontext.Roles.Where(s => roles.Contains(s.RoleName)).Select(s => s.Id).ToList();
            dbcontext.AddRange(roleIds.Select(g => new { RoleID = g, UserID = user.Id }));
            ret = dbcontext.SaveChanges() > 0;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public User? GetUserByUserName(string? userName)
    {
        User? user = null;
        if (userName != null)
        {
            using var dbcontext = DbFactory.CreateDbContext();
            user = dbcontext.Set<User>().FirstOrDefault(s => s.UserName == userName);
        }
        return user;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    public string? GetAppIdByUserName(string userName)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        return dbcontext.Set<User>().FirstOrDefault(s => s.UserName == userName)?.App;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool ChangePassword(string userName, string password, string newPassword)
    {
        var ret = false;
        if (Authenticate(userName, password))
        {
            using var dbcontext = DbFactory.CreateDbContext();
            var passSalt = LgbCryptography.GenerateSalt();
            password = LgbCryptography.ComputeHash(newPassword, passSalt);
            string sql = "update Users set Password = {0}, PassSalt = {1} where UserName = {2}";
            ret = dbcontext.Database.ExecuteSqlRaw(sql, new[] { password, passSalt, userName }) > 0;
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
        using var dbcontext = DbFactory.CreateDbContext();
        return dbcontext.Database.ExecuteSqlRaw("update Users set DisplayName = {1} where UserName = {0}", userName, displayName!) > 0;
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
        using var dbcontext = DbFactory.CreateDbContext();
        return dbcontext.Database.ExecuteSqlRaw("update Users set Css = {1} where UserName = {0}", userName, theme) > 0;
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
        using var dbcontext = DbFactory.CreateDbContext();
        return dbcontext.Database.ExecuteSqlRaw("update Users set Icon = {1} where UserName = {0}", userName, logo ?? "") > 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveApp(string userName, string app)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        return dbcontext.Database.ExecuteSqlRaw("update Users Set App = {1} Where UserName = {0}", userName, app) > 0;
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
        using var dbcontext = DbFactory.CreateDbContext();
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        var user = GetAll().FirstOrDefault(s => s.UserName == userName);
        bool ret;
        if (user == null)
        {
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
            dbcontext.Add(user);
            ret = dbcontext.SaveChanges() > 0;
            // 授权 Default 角色
            dbcontext.Database.ExecuteSqlRaw("insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = {0}", userName);
            ret = dbcontext.SaveChanges() > 0;
        }
        else
        {
            user.DisplayName = displayName;
            user.PassSalt = salt;
            user.Password = pwd;
            dbcontext.Update(user);
            ret = true;
        }
        return ret;
    }
}
