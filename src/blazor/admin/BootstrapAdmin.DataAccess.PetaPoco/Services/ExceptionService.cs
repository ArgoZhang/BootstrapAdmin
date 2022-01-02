using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class ExceptionService : BaseDatabase, IException
{
    public ExceptionService(IDatabase db) => Database = db;

    public bool Log(Error exception)
    {
        try
        {
            Database.Insert(exception);
        }
        catch { }
        return true;
    }
}
