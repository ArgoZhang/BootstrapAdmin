namespace BootstrapAdmin.Web.Pages.Account;

/// <summary>
/// 
/// </summary>
public partial class Login
{
    [SupplyParameterFromQuery]
    [Parameter]
    public string? ReturnUrl { get; set; }
}
