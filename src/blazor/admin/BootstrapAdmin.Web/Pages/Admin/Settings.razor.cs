using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Settings
{
    private bool IsDemo { get; set; }

    [NotNull]
    private AppInfo? AppInfo { get; set; }

    [NotNull]
    private List<SelectedItem>? Logins { get; set; }

    [NotNull]
    private List<SelectedItem>? Themes { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        IsDemo = DictService.IsDemo();
        Logins = DictService.GetLogins().ToSelectedItemList();
        Themes = DictService.GetThemes().ToSelectedItemList();
        AppInfo = new()
        {
            IsDemo = IsDemo,
            AuthCode = "123789",
            Title = DictService.GetWebTitle(),
            Footer = DictService.GetWebFooter(),
            Login = DictService.GetCurrentLogin(),
            SiderbarSetting = DictService.GetAppSiderbar(),
            TitleSetting = DictService.GetAppTitle(),
            FixHeaderSetting = DictService.GetAppFixHeader(),
            HealthCheckSetting = DictService.GetAppHealthCheck(),
        };
    }

    private async Task ShowToast(bool result, string title)
    {
        if (result)
        {
            await ToastService.Success(title, $"保存{title}成功");
        }
        else
        {
            await ToastService.Error(title, $"保存{title}失败");
        }
    }

    private async Task OnSaveTitle(EditContext context)
    {
        var ret = DictService.SaveWebTitle(AppInfo.Title);
        await ShowToast(ret, "网站标题");
    }

    private async Task OnSaveFooter(EditContext context)
    {
        var ret = DictService.SaveWebTitle(AppInfo.Title);
        await ShowToast(ret, "网站页脚");
    }

    private async Task OnSaveLogin(EditContext context)
    {
        var ret = DictService.SaveLogin(AppInfo.Login);
        await ShowToast(ret, "登录界面");
    }

    private async Task OnSaveTheme(EditContext context)
    {
        var ret = DictService.SaveLogin(AppInfo.Login);
        await ShowToast(ret, "网站主题");
    }

    private async Task OnSaveDemo(EditContext context)
    {
        var ret = false;
        if (DictService.AuthenticateDemo(AppInfo.AuthCode))
        {
            IsDemo = AppInfo.IsDemo;
            ret = DictService.SaveDemo(IsDemo);
            StateHasChanged();
        }
        await ShowToast(ret, "演示系统");
    }

    private async Task OnSaveApp(EditContext context)
    {
        var ret = DictService.SaveDemo(AppInfo.EnableDefaultApp);
        await ShowToast(ret, "默认应用");
    }

    private async Task OnSaveAppFeatures(EditContext context)
    {
        var ret = DictService.SaveAppSiderbar(AppInfo.SiderbarSetting);
        DictService.SaveAppTitle(AppInfo.TitleSetting);
        DictService.SaveAppFixHeader(AppInfo.FixHeaderSetting);
        DictService.SaveAppHealthCheck(AppInfo.HealthCheckSetting);
        await ShowToast(ret, "网站功能");
    }
}
