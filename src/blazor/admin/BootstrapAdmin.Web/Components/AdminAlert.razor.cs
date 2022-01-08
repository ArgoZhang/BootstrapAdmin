using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class AdminAlert
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Color Color { get; set; } = Color.Danger;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool IsShow { get; set; } = true;
}
