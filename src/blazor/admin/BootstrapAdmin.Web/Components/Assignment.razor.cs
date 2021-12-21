namespace BootstrapAdmin.Web.Components;

public partial class Assignment
{
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? Items { get; set; }

    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<string>? Value { get; set; }

    [Parameter]
    [EditorRequired]
    [NotNull]
    public Action<List<string>>? OnValueChanged { get; set; }

    private List<string> InternalValue
    {
        get { return Value; }
        set { Value = value; OnValueChanged(Value); }
    }
}
