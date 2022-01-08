namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class Assignment
{
    private List<string> InternalValue
    {
        get { return Value; }
        set { Value = value; OnValueChanged(Value); }
    }
}
