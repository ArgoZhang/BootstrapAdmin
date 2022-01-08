namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class UserLogin
{
    private string? UserName { get; set; }

    private string? Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

#if DEBUG
        UserName = "Admin";
        Password = "123789";
#endif
    }
}
