using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

public class RoleService : IRole
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory;

    public RoleService(IDbContextFactory<BootstrapAdminContext> dbFactory) => DbFactory = dbFactory;

    public List<Role> GetAll()
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.Roles.ToList();
    }

    public List<string> GetRolesByGroupId(string? groupId)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.RoleGroup.Where(s => s.GroupId == groupId).Select(s => s.RoleId).ToList();
    }

    public List<string> GetRolesByMenuId(string? menuId)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.NavigationRole.Where(s => s.NavigationId == menuId).Select(s => s.RoleId).ToList();
    }

    public List<string> GetRolesByUserId(string? userId)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.UserRole.Where(s => s.UserId == userId).Select(s => s.RoleId).ToList();
    }

    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var group = dbcontext.Groups.Include(s => s.Roles).Where(s => s.Id == groupId).FirstOrDefault();
        if (group != null)
        {
            group.Roles = dbcontext.Roles.Where(s => roleIds.Contains(s.Id)).ToList();
            return dbcontext.SaveChanges() > 0;
        }
        else
        {
            return false;
        }
    }

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var menu = dbcontext.Navigations.Include(s => s.Roles).Where(s => s.Id == menuId).FirstOrDefault();
        if (menu != null)
        {
            menu.Roles = dbcontext.Roles.Where(s => roleIds.Contains(s.Id)).ToList();
            return dbcontext.SaveChanges() > 0;
        }
        else
        {
            return false;
        }
    }

    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var user = dbcontext.Users.Include(s => s.Roles).Where(s => s.Id == userId).FirstOrDefault();
        if (user != null)
        {
            user.Roles = dbcontext.Roles.Where(s => roleIds.Contains(s.Id)).ToList();
            return dbcontext.SaveChanges() > 0;
        }
        else
        {
            return false;
        }
    }
}
