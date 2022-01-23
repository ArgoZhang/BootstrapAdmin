using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Logins
{
    /// <summary>
    /// 
    /// </summary>
    public ITableSearchModel TableSearchModel { get; } = new LoginLogModel();
}
