// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Utils;
using Longbow.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
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

    [NotNull]
    private AdminTable<HealthCheckReportItem>? HealthTable { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [NotNull]
    private HttpClient? Client { get; set; }

    private bool IsRunning { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Client = HttpClientFactory.CreateClient();
        Client.BaseAddress = new Uri(NavigationManager.BaseUri);

        IsRunning = true;
    }

    private async Task<QueryData<HealthCheckReportItem>> OnQueryAsync(QueryPageOptions options)
    {
        var payload = await Client.GetStringAsync("/Healths");
        var report = HealthCheckHelper.Parse(payload);

        var ret = new QueryData<HealthCheckReportItem>
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true,
            Items = report.Items
        };
        Duration = report.Duration;
        Status = report.Status;

        IsRunning = false;
        StateHasChanged();
        return ret;
    }

    private async Task OnCheck()
    {
        IsRunning = true;
        Duration = TimeSpan.Zero;
        StateHasChanged();
        await HealthTable.ToggleLoading(true);
        await HealthTable.QueryAsync();
        await HealthTable.ToggleLoading(false);
    }

    private static List<SelectedItem> GetNameLookup() => LookupHelper.GetCheckItems();

    private string? GetTagText(HealthStatus? status = null) => IsRunning ? "检查中 ..." : (status ?? Status) switch
    {
        HealthStatus.Healthy => "健康",
        HealthStatus.Degraded => "亚健康",
        _ => "不健康"
    };

    private Color GetTagColor(HealthStatus? status = null) => IsRunning ? Color.Success : (status ?? Status) switch
    {
        HealthStatus.Healthy => Color.Success,
        HealthStatus.Degraded => Color.Warning,
        _ => Color.Danger
    };

    private string? GetTagIcon(HealthStatus? status = null) => IsRunning ? "fa-solid fa-fw fa-spin fa-spinner" : (status ?? Status) switch
    {
        HealthStatus.Healthy => "fa-solid fa-check-circle",
        HealthStatus.Degraded => "fa-solid fa-exclamation-circle",
        _ => "fa-solid fa-times-circle"
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
