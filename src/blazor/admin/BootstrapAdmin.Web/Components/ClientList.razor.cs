// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ClientList
{
    [NotNull]
    private Dictionary<string, string>? Clients { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [NotNull]
    private DialogOption? Option { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Clients = DictService.GetClients();
    }

    private async Task OnSaveClient()
    {
        Option = new DialogOption
        {
            Title = "添加前台应用",
            Component = BootstrapDynamicComponent.CreateComponent<ClientDialog>(new Dictionary<string, object?>
            {
                [nameof(ClientDialog.Value)] = new ClientApp(),
                [nameof(ClientDialog.OnSave)] = new Func<ClientApp, Task>(app => OnSave(app)),
                [nameof(ClientDialog.OnClose)] = new Func<Task>(() => OnClose())
            }),
            ShowFooter = false,
        };
        await DialogService.Show(Option);
    }

    private async Task OnEditClient(string appId)
    {
        var client = DictService.GetClient(appId);
        Option = new DialogOption
        {
            Title = "编辑前台应用",
            Component = BootstrapDynamicComponent.CreateComponent<ClientDialog>(new Dictionary<string, object?>
            {
                [nameof(ClientDialog.Value)] = client,
                [nameof(ClientDialog.OnSave)] = new Func<ClientApp, Task>(app => OnSave(app)),
                [nameof(ClientDialog.OnClose)] = new Func<Task>(() => OnClose())
            }),
            ShowFooter = false,
        };
        await DialogService.Show(Option);
    }

    private Task OnDeleteClient(string appId)
    {
        DictService.DeleteClient(appId);
        Clients = DictService.GetClients();
        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnClose() => Option.CloseDialogAsync();

    private async Task OnSave(ClientApp app)
    {
        DictService.SaveClient(app);
        await Option.CloseDialogAsync();
        Clients = DictService.GetClients();
        StateHasChanged();
    }
}
