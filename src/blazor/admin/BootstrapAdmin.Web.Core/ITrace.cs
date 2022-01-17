using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface ITrace
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="trace"></param>
    public void Log(Trace trace);
}
