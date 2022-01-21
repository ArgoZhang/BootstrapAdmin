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
    public ITableSearchModel TableSearchModel { get; set; } = new LoginLogModel();

    private List<string> SortList { get; set; } = new List<string>() { "LoginTime desc" };
}
