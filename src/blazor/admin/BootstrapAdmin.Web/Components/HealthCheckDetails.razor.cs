using Bootstrap.Security.Blazor.HealthChecks;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class HealthCheckDetails
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public IDictionary<string, object?>? Data { get; set; }

    [NotNull]
    private List<SelectedItem>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Items = Data.Select(d => new SelectedItem(d.Key, d.Value?.ToString() ?? "")).ToList();
    }

    private static MarkupString GetText(string? value)
    {
        var ret = value ?? "";
        ret = ret.Replace("\n", "<br />");
        return new MarkupString(ret);
    }
}
