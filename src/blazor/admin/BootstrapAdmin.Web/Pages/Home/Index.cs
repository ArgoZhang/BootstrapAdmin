using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace BootstrapAdmin.Web.Pages.Home;

/// <summary>
/// 返回前台页面
/// </summary>
[Route("/")]
[Route("/Index")]
[Route("/Home/Index")]
[Authorize]
public class Index : ComponentBase
{
    [Inject]
    [NotNull]
    private NavigationManager? Navigation { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UsersService { get; set; }

    [NotNull]
    private string? Url { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        var appId = UsersService.GetAppIdByUserName(Context.UserName);
        Url = DictsService.GetHomeUrlByAppId(appId) ?? "Admin/Index";

#if !DEBUG
        Navigation.NavigateTo(Url, true);
#endif
    }

#if DEBUG
    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstRender"></param>
    protected override void OnAfterRender(bool firstRender)
    {
        Navigation.NavigateTo(Url, true);
    }
#endif
}
