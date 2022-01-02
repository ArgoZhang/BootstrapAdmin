using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class ExceptionService : IException
{
    public bool Log(Models.Exception exception)
    {
        return true;
    }
}
