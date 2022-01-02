using BootstrapAdmin.Web.Services;
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

    /// <summary>
    /// OnInitializedAsync
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var client = HttpClientFactory.CreateClient();
        client.BaseAddress = new Uri(NavigationManager.BaseUri);
        var payload = await client.GetStringAsync("/Healths");

        var serializeOption = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            PropertyNameCaseInsensitive = true
        };
        serializeOption.Converters.Add(new StringToTimeSpanConverter());

        var report = JsonSerializer.Deserialize<dynamic>(payload, serializeOption);
        if (report != null)
        {
            foreach (var entry in report.Keys)
            {
                var item = report.Entries[entry];
            }
        }
    }
}
