using Bootstrap.Security.Blazor;
using BootstrapBlazor.Components;
using BootstrapClient.Web.Core;
using BootstrapClient.Web.Shared.Extensions;
using BootstrapClient.Web.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BootstrapClient.Web.Shared.Shared;

/// <summary>
/// 
/// </summary>
public sealed partial class MainLayout
{
    private bool UseTabSet { get; set; } = true;

    private string Theme { get; set; } = "";

    private bool IsOpen { get; set; }

    private bool IsFixedHeader { get; set; } = true;

    private bool IsFixedFooter { get; set; } = true;

    private bool IsFullSide { get; set; } = true;

    private bool ShowFooter { get; set; } = true;

    private IEnumerable<MenuItem>? MenuItems { get; set; }

    private string? ProfileUrl { get; set; }

    private string? SettingsUrl { get; set; }

    private string? NotificationUrl { get; set; }

    private string? Title { get; set; }

    private string? Footer { get; set; }

    [Inject]
    [NotNull]
    private IBootstrapAdminService? SecurityService { get; set; }

    [Inject]
    [NotNull]
    private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UsersService { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationsService { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Context.BaseUri = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);

        var adminUrl = DictsService.GetAdminUrl();
        if (!string.IsNullOrEmpty(adminUrl))
        {
            Context.AdminUrl = string.Format(adminUrl, Context.BaseUri.Scheme, Context.BaseUri.Host).TrimEnd('/');
        }

        ProfileUrl = CombinePath(DictsService.GetProfileUrl(Context.AppId));
        SettingsUrl = CombinePath(DictsService.GetSettingsUrl(Context.AppId));
        NotificationUrl = CombinePath(DictsService.GetNotificationUrl(Context.AppId));
    }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userName = state.User.Identity?.Name;

        if (!string.IsNullOrEmpty(userName))
        {
            Context.UserName = userName;
            var user = UsersService.GetUserByUserName(userName);

            MenuItems = NavigationsService.GetMenus(userName).Where(i => i.Application == Context.AppId).ToMenus();

            Context.DisplayName = user?.DisplayName ?? "未注册账户";
            Title = DictsService.GetWebTitle(Context.AppId);
            Footer = DictsService.GetWebFooter(Context.AppId);
        }
    }

    private Task<bool> OnAuthorizing(string url)
    {
        return SecurityService.AuhorizingNavigation(Context.UserName, url);
    }

    private string LogoutUrl => CombinePath($"/Account/Logout?AppId={Context.AppId}");

    private string AuthorUrl => CombinePath($"/Account/Login?ReturnUrl={NavigationManager.Uri}&AppId={Context.AppId}");

    private string CombinePath(string? url)
    {
        url ??= "";
        if (!string.IsNullOrEmpty(Context.AdminUrl))
        {
            url = string.Join('/', Context.AdminUrl, url.TrimStart('/'));
        }
        else
        {
            url = "#";
        }
        return url;
    }
}
