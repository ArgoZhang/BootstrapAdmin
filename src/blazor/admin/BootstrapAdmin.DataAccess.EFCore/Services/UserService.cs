using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

public class UserService : IUser
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory { get; set; }

    public UserService(IDbContextFactory<BootstrapAdminContext> factory) => DbFactory = factory;

    public List<User> GetAll()
    {
        using var context = DbFactory.CreateDbContext();
        return context.Users.ToList();
    }

    public bool Authenticate(string userName, string password)
    {
        using var context = DbFactory.CreateDbContext();

        var user = context.Users.Where(s => s.ApprovedTime != null).FirstOrDefault(x => x.UserName == userName);

        var isAuth = false;
        if (user != null && !string.IsNullOrEmpty(user.PassSalt))
        {
            isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }
        return isAuth;
    }

    public List<string> GetApps(string userName)
    {
        return new List<string> { "BA" };
    }

    public string? GetDisplayName(string? userName)
    {
        using var context = DbFactory.CreateDbContext();
        return string.IsNullOrEmpty(userName) ? "" : context.Users.FirstOrDefault(s => s.UserName == userName)?.DisplayName;
    }

    public List<string> GetRoles(string userName)
    {
        using var context = DbFactory.CreateDbContext();

        var user = context.Users.Include(s => s.Roles).FirstOrDefault(s => s.UserName == userName);

        return user != null ? user?.Roles?.Select(s => s.RoleName).ToList() : new List<string>();
    }

    public List<string> GetUsersByGroupId(string? groupId)
    {
        using var context = DbFactory.CreateDbContext();

        return context.UserGroup.Where(s => s.GroupId == groupId).Select(s => s.UserId).ToList();
    }

    public List<string> GetUsersByRoleId(string? roleId)
    {
        using var context = DbFactory.CreateDbContext();

        return context.UserRole.Where(s => s.RoleId == roleId).Select(s => s.UserId).ToList();
    }

    public bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var group = dbcontext.Groups.Include(s => s.Users).Where(s => s.Id == groupId).FirstOrDefault();
        if (group != null)
        {
            group.Users = dbcontext.Users.Where(s => userIds.Contains(s.Id)).ToList();
            return dbcontext.SaveChanges() > 0;
        }
        else
        {
            return false;
        }
    }

    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var currentrole = dbcontext.Roles.Include(s => s.Users).Where(s => s.Id == roleId).FirstOrDefault();
        if (currentrole != null)
        {
            currentrole.Users = dbcontext.Users.Where(s => userIds.Contains(s.Id)).ToList();
            return dbcontext.SaveChanges() > 0;
        }
        else
        {
            return false;
        }
    }

    public bool TryCreateUserByPhone(string phone, string appId, ICollection<string> roles)
    {
        var ret = false;
        using var dbcontext = DbFactory.CreateDbContext();
        try
        {
            var user = GetAll().FirstOrDefault(user => user.UserName == phone);
            if (user == null)
            {
                dbcontext.Database.BeginTransaction();
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
            ret = true;
        }
        catch (Exception)
        {

            throw;
        }
        return ret;
    }
}
