// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;

namespace BootstrapAdmin.DataAccess.SqlSugar.Service;

class UserService : IUser
{
    private ISqlSugarClient db { get; }

    public UserService(ISqlSugarClient db) => this.db = db;

    public bool Authenticate(string userName, string password)
    {
        var user = db.Queryable<User>()
            .Where(s => s.ApprovedTime != null && s.UserName == userName)
            .Select(s => new User
            {
                DisplayName = s.DisplayName,
                PassSalt = s.PassSalt,
                Password = s.Password
            }).Single();

        var isAuth = false;
        if (user != null && !string.IsNullOrEmpty(user.PassSalt))
        {
            isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }
        return isAuth;
    }

    public List<User> GetAll()
    {
        return db.Queryable<User>().ToList();
    }

    public List<string> GetApps(string userName)
    {
        return db.Ado.SqlQuery<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName) r on ra.RoleId = r.ID union select Code from Dicts where Category = @Category and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName)", new { UserName = userName, Category = "应用程序", RoleName = "Administrators" });
    }

    /// <summary>
    /// 通过用户名获得指定的前台 AppId
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetAppIdByUserName(string userName) =>
    //db.Ado.SqlQuery<User>("select * from users where username=@userName", new { userName }).Select(s => s.App).Single();
    db.Queryable<User>().Where(s => s.UserName == userName).Select(s => s.App).Single();

    public string? GetDisplayName(string? userName)
    {
        return db.Queryable<User>().Where(s => s.UserName == userName).Select(s => s.DisplayName).Single();
    }

    public List<string> GetRoles(string userName)
    {
        //var roles = db.Ado.SqlQuery<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @userName union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName = @userName", new { userName });

        //转为 linq写法
        var roles = db.Union(
            db.Queryable<Role>()
            .InnerJoin<UserRole>((r, ur) => r.Id == ur.RoleID)
            .InnerJoin<User>((r, ur, u) => ur.UserID == u.Id && u.UserName == userName)
            .Select(r => r.RoleName),
            db.Queryable<Role>()
            .InnerJoin<RoleGroup>((r, rg) => r.Id == rg.RoleID)
            .InnerJoin<Group>((r, rg, g) => rg.GroupID == g.Id)
            .InnerJoin<UserGroup>((r, rg, g, ug) => ug.GroupID == g.Id)
            .InnerJoin<User>((r, rg, g, ug, u) => ug.UserID == u.Id && u.UserName == userName)
            .Select(r => r.RoleName)
        ).ToList();
        return roles;
    }

    public User? GetUserByUserName(string? userName) =>
        string.IsNullOrEmpty(userName) ? null :
        // db.Ado.SqlQuery<User>("select * from users where username=@userName", new { userName }).Single();
        //转linq写法
        db.Queryable<User>()
        .Where(t => t.UserName == userName).Single();


    public List<string> GetUsersByGroupId(string? groupId)
    {
        //return db.Ado.SqlQuery<string>("select UserID from UserGroup where GroupID = @groupId", new { groupId });
        //转linq写法
        return db.Queryable<UserGroup>()
            .Where(t => t.GroupID == groupId)
            .Select(t => t.UserID)
            .ToList()
            .Select(t => t ?? "") //这个转换本来没有意义，但是 UserID Model自动中存储的是可空  做个转换  避免vs提示错误          
            .ToList();
    }

    public List<string> GetUsersByRoleId(string? roleId)
    {
        //return db.Ado.SqlQuery<string>("select UserID from UserRole where RoleID = @roleId", new { roleId });

        return db.Queryable<UserRole>()
            .Where(t => t.RoleID == roleId)
            .Select(t => t.UserID)
            .ToList()
            .Select(t => t ?? "")//这个转换本来没有意义，但是 UserID Model自动中存储的是可空  做个转换  避免vs提示错误 
            .ToList();
    }

    public bool SaveUser(string userName, string displayName, string password)
    {
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        var user = db.Queryable<User>().Where(s => s.UserName == userName).Single();
        bool ret = default;
        if (user == null)
        {
            // 开始事务
            db.Ado.BeginTran();
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
            db.Insertable(user).ExecuteCommand();
            // 授权 Default 角色
            //db.Ado.ExecuteCommand("insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = @userName", new { userName });

            //改linq写法

            var urs = db.Queryable<User>()
                .Where(t => t.UserName == userName)
                .Select(t => new UserRole()
                {
                    UserID = t.Id,
                    RoleID = SqlFunc.Subqueryable<Role>().Where(x => x.RoleName == "Default").Select(x => x.Id)
                }).ToList();

            db.Insertable(urs).ExecuteCommand();

            ret = true;
            db.Ado.CommitTran();
        }
        else
        {
            user.DisplayName = displayName;
            user.PassSalt = salt;
            user.Password = pwd;
            db.Updateable(user).ExecuteCommand();
            ret = true;
        }
        return ret;
    }

    public bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            //db.Ado.ExecuteCommand("delete from UserGroup where GroupId = @groupId", new { groupId });
            db.Deleteable<UserGroup>().Where(x => x.GroupID == groupId).ExecuteCommand();
            db.Insertable<UserGroup>(userIds.Select(g => new UserGroup { UserID = g, GroupID = groupId })).ExecuteCommand();
            db.Ado.CommitTran();

            ret = true;
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        var ret = false;
        try
        {
            db.Ado.BeginTran();
            //db.Ado.ExecuteCommand("delete from UserRole where RoleID = @roleId", new { roleId });
            db.Deleteable<UserRole>().Where(x => x.RoleID == roleId).ExecuteCommand();
            db.Insertable<UserRole>(userIds.Select(g => new UserRole { UserID = g, RoleID = roleId })).ExecuteCommand();
            ret = true;
            db.Ado.CommitTran();
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
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
            var user = db.Queryable<User>().Where(s => s.UserName == phone).Single();
            if (user == null)
            {
                db.Ado.BeginTran();
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
                db.Insertable(user).ExecuteCommand();
                // Authorization
                //var roleIds = db.Ado.SqlQuery<string>("select ID from Roles where RoleName in (@roles)", new { roles });
                var roleIds = db.Queryable<Role>().Where(t => roles.Contains(t.RoleName)).Select(t => t.Id).ToList();
                db.Insertable<UserRole>(roleIds.Select(g => new UserRole { RoleID = g, UserID = user.Id })).ExecuteCommand();
                db.Ado.CommitTran();
            }
            else
            {
                user.PassSalt = salt;
                user.Password = pwd;
                db.Updateable(user).ExecuteCommand();
            }
            ret = true;
        }
        catch (Exception)
        {
            db.Ado.RollbackTran();
            throw;
        }
        return ret;
    }

    public bool SaveApp(string userName, string app)
    {
        throw new NotImplementedException();
    }

    public bool ChangePassword(string userName, string password, string newPassword)
    {
        throw new NotImplementedException();
    }

    public bool SaveDisplayName(string userName, string displayName)
    {
        throw new NotImplementedException();
    }

    public bool SaveTheme(string userName, string theme)
    {
        throw new NotImplementedException();
    }

    public bool SaveLogo(string userName, string? logo)
    {
        throw new NotImplementedException();
    }
}
