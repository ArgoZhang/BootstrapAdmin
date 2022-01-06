using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class UserService : IUser
{
    private IFreeSql FreeSql;

    public UserService(IFreeSql freeSql) => FreeSql = freeSql;

    public bool Authenticate(string userName, string password)
    {
        return true;
    }

    public List<User> GetAll()
    {
        return FreeSql.Select<User>().ToList();
    }

    public List<string> GetApps(string userName)
    {
        return new List<string> { "BA" };
    }

    public string? GetDisplayName(string? userName)
    {
        return "Admin";
    }

    public List<string> GetRoles(string userName)
    {
        return new List<string> { "Default" };
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

    public bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles)
    {
        throw new NotImplementedException();
    }
}
