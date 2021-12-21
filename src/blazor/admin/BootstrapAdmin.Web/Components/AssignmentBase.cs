namespace BootstrapAdmin.Web.Components;

public abstract class AssignmentBase<TItem> : ComponentBase
{
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<TItem>? Items { get; set; }

    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<string>? Value { get; set; }

    [Parameter]
    [EditorRequired]
    [NotNull]
    public Action<List<string>>? OnValueChanged { get; set; }
}
