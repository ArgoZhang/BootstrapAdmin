using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class TraceService : ITrace
{
    private IDatabase Database { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public TraceService(IDatabase db) => Database = db;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trace"></param>
    public void Log(Trace trace)
    {
        Database.Insert(trace);
    }
}
