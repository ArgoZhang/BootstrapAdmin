namespace BootstrapAdmin.Web.Components;

public partial class Assignment
{
    private List<string> InternalValue
    {
        get { return Value; }
        set { Value = value; OnValueChanged(Value); }
    }
}
