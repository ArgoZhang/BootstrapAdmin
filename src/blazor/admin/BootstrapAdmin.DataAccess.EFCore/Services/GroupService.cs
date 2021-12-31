using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.EFCore.Services;

public class GroupService : IGroup
{
    private IDbContextFactory<BootstrapAdminContext> DbFactory;

    public GroupService(IDbContextFactory<BootstrapAdminContext> dbFactory) => DbFactory = dbFactory;

    public List<Group> GetAll()
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.Groups.ToList();
    }

    public List<string> GetGroupsByRoleId(string? roleId)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.RoleGroup.Where(s => s.RoleId == roleId).Select(s => s.GroupId!).ToList();
    }

    public List<string> GetGroupsByUserId(string? userId)
    {
        using var dbcontext = DbFactory.CreateDbContext();

        return dbcontext.UserGroup.Where(s => s.UserId == userId).Select(s => s.GroupId!).ToList();
    }

    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var role = dbcontext.Roles.Include(s => s.Groups).Where(s => s.Id == roleId).FirstOrDefault();
        if (role != null)
        {
            role.Groups = dbcontext.Groups.Where(s => groupIds.Contains(s.Id)).ToList();
            return dbcontext.SaveChanges() > 0;
        }
        else
        {
            return false;
        }
    }

    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        using var dbcontext = DbFactory.CreateDbContext();
        var user = dbcontext.Users.Include(s => s.Groups).Where(s => s.Id == userId).FirstOrDefault();
        if (user != null)
        {
            user.Groups = dbcontext.Groups.Where(s => groupIds.Contains(s.Id)).ToList();
            return dbcontext.SaveChanges() > 0;
        }
        else
        {
            return false;
        }
    }
}
