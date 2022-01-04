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

    private string? PostUrl { get; set; }

    [SupplyParameterFromQuery]
    [Parameter]
    public string? ReturnUrl { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    private string? UserName { get; set; }

    private string? Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = DictsService.GetWebTitle();

        PostUrl = QueryHelper.AddQueryString("/Account/Login", "ReturnUrl", ReturnUrl ?? "");

#if DEBUG
        UserName = "Admin";
        Password = "123789";
#endif
    }

    void OnClickSwitchButton()
    {
        var rem = RememberPassword ? "true" : "false";
        PostUrl = QueryHelper.AddQueryString(UseMobileLogin ? "/Account/Mobile" : "/Account/Login", new Dictionary<string, string?>()
        {
            [nameof(ReturnUrl)] = ReturnUrl,
            ["remember"] = rem
        });
    }

    Task OnRememberPassword(bool remember)
    {
        OnClickSwitchButton();
        return Task.CompletedTask;
    }

    void OnSignUp()
    {

    }

    void OnForgotPassword()
    {

    }
}
