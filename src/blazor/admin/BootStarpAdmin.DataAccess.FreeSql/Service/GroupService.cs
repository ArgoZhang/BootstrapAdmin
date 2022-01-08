using BootStarpAdmin.DataAccess.FreeSql.Models;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

public class GroupService : IGroup
{
    private IFreeSql FreeSql;

    public GroupService(IFreeSql freeSql) => FreeSql = freeSql;

    public List<Group> GetAll() => FreeSql.Select<Group>().ToList();

    public List<string> GetGroupsByRoleId(string? roleId) => FreeSql.Ado.Query<string>("select GroupID from RoleGroup where RoleID = @roleId", new { roleId = roleId });

    public List<string> GetGroupsByUserId(string? userId) => FreeSql.Ado.Query<string>("select GroupID from UserGroup where UserID = @userId", new { userId = userId });

    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from RoleGroup where RoleID = @roleId", new { roleId = roleId });
                FreeSql.Insert(groupIds.Select(g => new RoleGroup { GroupID = g, RoleID = roleId })).ExecuteAffrows();
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }

    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        var ret = false;
        try
        {
            FreeSql.Transaction(() =>
            {
                FreeSql.Ado.ExecuteNonQuery("delete from UserGroup where UserID = @userId", new { userId = userId });
                FreeSql.Insert(groupIds.Select(g => new UserGroup { GroupID = g, UserID = userId }));
                ret = true;
            });
        }
        catch (Exception)
        {
            throw;
        }
        return ret;
    }
}
