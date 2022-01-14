using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Pages.Admin;

public partial class Logins
{
    /// <summary>
    /// 
    /// </summary>
    public ITableSearchModel TableSearchModel { get; set; } = new LoginLogModel();
}
