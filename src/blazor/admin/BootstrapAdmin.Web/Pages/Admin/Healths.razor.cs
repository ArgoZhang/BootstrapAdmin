using Bootstrap.Security.Blazor.HealthChecks;
using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Utils;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BootstrapAdmin.Web.Pages.Admin;

public partial class Healths
{
    [Inject]
    [NotNull]
    private IHttpClientFactory? HttpClientFactory { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    private TimeSpan Duration { get; set; }

    private HealthStatus Status { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [NotNull]
    private HttpClient? Client { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Client = HttpClientFactory.CreateClient();
        Client.BaseAddress = new Uri(NavigationManager.BaseUri);
    }

    private async Task<QueryData<HealthCheckReportItem>> OnQueryAsync(QueryPageOptions options)
    {
        var payload = await Client.GetStringAsync("/Healths");
        var report = HealthCheckHelper.Parse(payload);

        var ret = new QueryData<HealthCheckReportItem>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        ret.Items = report.Items;
        Duration = report.Duration;
        Status = report.Status;

        return ret;
    }

    private static List<SelectedItem> GetNameLookup() => LookupHelper.GetCheckItems();

    private string? GetTagText(HealthStatus? status = null) => (status ?? Status) switch
    {
        HealthStatus.Healthy => "健康",
        HealthStatus.Degraded => "亚健康",
        _ => "不健康"
    };

    private Color GetTagColor(HealthStatus? status = null) => (status ?? Status) switch
    {
        HealthStatus.Healthy => Color.Success,
        HealthStatus.Degraded => Color.Warning,
        _ => Color.Danger
    };

    private string? GetTagIcon(HealthStatus? status = null) => (status ?? Status) switch
    {
        HealthStatus.Healthy => "fa fa-check-circle",
        HealthStatus.Degraded => "fa fa-exclamation-circle",
        _ => "fa fa-times-circle"
    };

    private Task OnRowButtonClick(HealthCheckReportItem item) => DialogService.Show(new DialogOption()
    {
        Title = $"{LookupHelper.GetCheckItems().FirstOrDefault(i => i.Value == item.Name)?.Text} - 详细数据",
        IsScrolling = true,
        Component = BootstrapDynamicComponent.CreateComponent<HealthCheckDetails>(new Dictionary<string, object?>
        {
            { nameof(HealthCheckDetails.Data), item.Data }
        })
    });
}
