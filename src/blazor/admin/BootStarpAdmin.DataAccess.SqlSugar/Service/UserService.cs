using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootStarpAdmin.DataAccess.SqlSugar.Service;

/// <summary>
/// 
/// </summary>
public class UserService : IUser
{
    private SqlSugarClient Client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    public UserService(SqlSugarClient client) => Client = client;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<User> GetAll()
    {
        return Client.Queryable<User>().ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool Authenticate(string userName, string password)
    {
        var user = Client.Ado.SqlQuery<User>("select DisplayName, Password, PassSalt from Users where ApprovedTime is not null and UserName = @UserName").First();

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
            string sql = "set Password = @Password, PassSalt = @PassSalt where UserName = @UserName";
            ret = Client.Ado.ExecuteCommand(sql, new { Password = password, PassSalt = passSalt, UserName = userName }) == 1;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetAppIdByUserName(string userName)
    {
        return Client.Queryable<User>().Where(s => s.UserName == userName).First()?.App;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetApps(string userName)
    {
        return Client.Ado.SqlQuery<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName) r on ra.RoleId = r.ID union select Code from Dicts where Category = @Category and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @UserName and r.RoleName = @RoleName)", new { UserName = userName, Category = "应用程序", RoleName = "Administrators" });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetRoles(string userName)
    {
        return Client.Ado.SqlQuery<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @0 union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join [Groups] g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName=@UserName", new { UserName = userName });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public User? GetUserByUserName(string? userName) => string.IsNullOrEmpty(userName) ? null : Client.Queryable<User>().Where(s => s.UserName == userName).First();

    public List<string> GetUsersByGroupId(string? groupId)
    {
        throw new NotImplementedException();
    }

    public List<string> GetUsersByRoleId(string? roleId)
    {
        throw new NotImplementedException();
    }

    public bool SaveApp(string userName, string app)
    {
        throw new NotImplementedException();
    }

    public bool SaveDisplayName(string userName, string displayName)
    {
        throw new NotImplementedException();
    }

    public bool SaveLogo(string userName, string? logo)
    {
        throw new NotImplementedException();
    }

    public bool SaveTheme(string userName, string theme)
    {
        throw new NotImplementedException();
    }

    public bool SaveUser(string userName, string displayName, string password)
    {
        throw new NotImplementedException();
    }

    public bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds)
    {
        throw new NotImplementedException();
    }

    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        throw new NotImplementedException();
    }

    public bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles)
    {
        throw new NotImplementedException();
    }
}
