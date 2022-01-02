namespace BootstrapAdmin.Web.Core;

public interface IException
{
    bool Log(DataAccess.Models.Exception exception);
}
