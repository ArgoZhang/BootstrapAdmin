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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public List<string> GetUsersByRoleId(string? roleId)
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

    public bool TryCreateUserByPhone(string phone, string appId, ICollection<string> roles)
    {
        throw new NotImplementedException();
    }
}
