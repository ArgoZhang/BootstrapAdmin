using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class AppService : BaseDatabase, IApp
{
    public AppService(IDatabase db)
    {
        Database = db;
    }

    public List<string> GetAppsByRoleId(string? roleId) => Database.Fetch<string>("select AppID from RoleApp where RoleID = @0", roleId);

    public bool SaveAppsByRoleId(string? roleId, IEnumerable<string> appIds)
    {
        var ret = false;
        try
        {
            Database.BeginTransaction();
            Database.Execute("delete from RoleApp where RoleID = @0", roleId);
            Database.InsertBatch("RoleApp", appIds.Select(g => new { AppID = g, RoleID = roleId }));
            Database.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            Database.AbortTransaction();
            throw;
        }
        return ret;
    }
}
