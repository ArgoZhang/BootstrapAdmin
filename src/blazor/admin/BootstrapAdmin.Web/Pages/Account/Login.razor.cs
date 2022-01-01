using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Pages.Account;

/// <summary>
/// 
/// </summary>
public partial class Login
{
    private string? Title { get; set; }

    private bool AllowMobile { get; set; } = true;

    private bool UseMobileLogin { get; set; }

    private bool AllowOAuth { get; set; } = true;

    private bool RememberPassword { get; set; }

    private string? PostUrl { get; set; } = "/Account/Login";

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = DictsService.GetWebTitle();
    }

    void OnClickSwitchButton()
    {
        var rem = RememberPassword ? "true" : "false";
        PostUrl = UseMobileLogin ? $"/Account/Mobile?remember={rem}" : $"/Account/Login?remember={rem}";
    }

    Task OnRememberPassword(bool remember)
    {
        var rem = remember ? "true" : "false";
        PostUrl = UseMobileLogin ? $"/Account/Mobile?remember={rem}" : $"/Account/Login?remember={rem}";
        return Task.CompletedTask;
    }

    void OnSignUp()
    {

    }

    void OnForgotPassword()
    {

    }
}
