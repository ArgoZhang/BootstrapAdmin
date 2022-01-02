using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

class ExceptionService : IException
{
    public bool Log(Error exception)
    {
        return true;
    }
}
