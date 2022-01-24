using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web;

/// <summary>
/// 
/// </summary>
public partial class App
{
    /// <summary>
    /// 
    /// </summary>
    public string? Title { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        AppContext.BaseUri = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);
    }
}
