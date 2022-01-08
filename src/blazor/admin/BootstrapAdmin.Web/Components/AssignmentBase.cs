namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TItem"></typeparam>
public abstract class AssignmentBase<TItem> : ComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<TItem>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<string>? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Action<List<string>>? OnValueChanged { get; set; }
}
