using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Components;

public partial class AdminAlert
{
    [Parameter]
    public string? Text { get; set; }

    private bool IsDemo { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        IsDemo = DictService.IsDemo();
    }
}
