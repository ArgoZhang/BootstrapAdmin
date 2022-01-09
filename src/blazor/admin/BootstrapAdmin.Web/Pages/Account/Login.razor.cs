namespace BootstrapAdmin.Web.Pages.Account;

/// <summary>
/// 
/// </summary>
public partial class Login
{
    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery]
    [Parameter]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery]
    [Parameter]
    public string? AppId { get; set; }
}
