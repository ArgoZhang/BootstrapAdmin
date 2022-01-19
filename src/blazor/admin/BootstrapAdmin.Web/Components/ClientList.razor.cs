using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ClientList
{
    [NotNull]
    private Dictionary<string, string>? Client { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [NotNull]
    private DialogOption? Option { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Client = DictService.GetFrontApp();
    }

    private async Task OnSaveClient()
    {
        Option = new DialogOption
        {
            Title = "添加前台应用",
            BodyTemplate = BootstrapDynamicComponent.CreateComponent<ClientDialog>(new Dictionary<string, object?>
            {
                [nameof(ClientDialog.OnSaveComplete)] = new Func<AppInfo, Task>(e => OnSave(e)),
                [nameof(ClientDialog.OnClose)] = new Func<Task>(() => OnClose())
            }).Render(),
            ShowFooter = false,
        };
        await DialogService.Show(Option);
    }

    private async Task OnEditClient(string appID, string appName)
    {
        var frontapp = DictService.GetFrontAppSettings(appID, appName);
        var appInfo = new AppInfo()
        {
            AppID = appID,
            AppName = appName,
            Home = frontapp.homeurl,
            WebTitle = frontapp.title,
            WebFooter = frontapp.footer,
            WebIcon = frontapp.icon,
            Favicon = frontapp.favicon,
        };

        Option = new DialogOption
        {
            Title = "编辑前台应用",
            BodyTemplate = BootstrapDynamicComponent.CreateComponent<ClientDialog>(new Dictionary<string, object?>
            {
                [nameof(ClientDialog.Value)] = appInfo,
                [nameof(ClientDialog.OnSaveComplete)] = new Func<AppInfo, Task>(e => OnSave(e)),
                [nameof(ClientDialog.OnClose)] = new Func<Task>(() => OnClose())
            }).Render(),
            ShowFooter = false,
        };
        await DialogService.Show(Option);
    }

    private Task OnDeleteClient(string appID, string appName)
    {
        DictService.DeleteFrontAppSettings(appID, appName);
        return Task.CompletedTask;
    }

    private async Task OnClose()
    {
        await Option.Dialog.Close();
    }

    private async Task OnSave(AppInfo Value)
    {
        DictService.SaveFrontApp(Value.AppID, Value.AppName, Value.Home, Value.WebTitle, Value.WebFooter, Value.WebIcon, Value.Favicon);
        await Option.Dialog.Close();
    }
}
