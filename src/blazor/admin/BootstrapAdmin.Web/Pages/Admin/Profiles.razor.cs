using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapAdmin.Web.Pages.Admin;

public partial class Profiles
{
    public bool IsDemo { get; set; }

    [NotNull]
    private User? CurrentUser { get; set; }

    [Inject]
    [NotNull]
    public BootstrapAppContext? AppContext { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        CurrentUser = new User()
        {
            App = AppContext.AppId,
            UserName = AppContext.UserName,
            DisplayName = AppContext.DisplayName
        };
    }

    private Task OnSaveDisplayName(EditContext context)
    {
        return Task.CompletedTask;
    }
}
