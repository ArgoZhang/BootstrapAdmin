// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapClient.DataAccess.Models;
using BootstrapClient.Web.Core;
using PetaPoco;

namespace BootstrapClient.DataAccess.PetaPoco.Services;

class UserService : IUser
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public UserService(IDBManager db) => DBManager = db;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public User? GetUserByUserName(string? userName)
    {
        User? user = null;
        if (!string.IsNullOrEmpty(userName))
        {
            using var db = DBManager.Create();
            user = db.FirstOrDefault<User>("Where UserName = @0", userName);
        }
        return user;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetApps(string userName)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0) r on ra.RoleId = r.ID union select Code from Dicts where Category = @1 and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 and r.RoleName = @2 union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0 and r.RoleName = @2)", userName, "应用程序", "Administrators");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetRoles(string userName)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @0 union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName=@0", userName);
    }
}
