using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

public interface IException
{
    bool Log(Error exception);
}
