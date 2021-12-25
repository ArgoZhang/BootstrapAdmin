using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Components;

public partial class AdminAlert
{
    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public Color Color { get; set; } = Color.Danger;

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
