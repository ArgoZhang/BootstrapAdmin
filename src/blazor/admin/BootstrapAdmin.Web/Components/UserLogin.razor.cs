namespace BootstrapAdmin.Web.Components;

public partial class UserLogin
{
    private string? UserName { get; set; }

    private string? Password { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

#if DEBUG
        UserName = "Admin";
        Password = "123789";
#endif
    }
}
