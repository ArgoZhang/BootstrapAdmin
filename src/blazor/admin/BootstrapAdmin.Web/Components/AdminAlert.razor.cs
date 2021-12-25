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

    [Parameter]
    public bool IsShow { get; set; } = true;
}
