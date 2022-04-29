using BootStarpAdmin.DataAccess.SqlSugar.Models;
using BootstrapAdmin.Web.Core;
using SqlSugar;

namespace BootStarpAdmin.DataAccess.SqlSugar.Service;

/// <summary>
/// 
/// </summary>
public class AppService : IApp
{
    private ISqlSugarClient Client { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    public AppService(ISqlSugarClient client) => Client = client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetAppsByRoleId(string? roleId)
    {
        return Client.Ado.SqlQuery<string>("select AppID from RoleApp where RoleID = @roleId", new { roleId = roleId });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="appIds"></param>
    /// <returns></returns>
    public bool SaveAppsByRoleId(string? roleId, IEnumerable<string> appIds)
    {
        var ret = false;
        try
        {
            Client.Ado.BeginTran();
            Client.Ado.ExecuteCommand("delete from RoleApp where RoleID = @roleId", new { roleId = roleId });
            Client.Insertable(appIds.Select(g => new RoleApp { AppID = g, RoleID = roleId }).ToList()).ExecuteCommand();
            Client.Ado.CommitTran();
            ret = true;
        }
        catch (Exception)
        {
            Client.Ado.RollbackTran();
            throw;
        }
        return ret;
    }
}
