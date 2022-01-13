using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Profiles
{
    private bool IsDemo { get; set; }

    [NotNull]
    private User? CurrentUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Inject]
    [NotNull]
    public BootstrapAppContext? AppContext { get; set; }

    [NotNull]
    private List<SelectedItem>? Apps { get; set; }

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

    private List<UploadFile> PreviewFileList { get; } = new(new[] { new UploadFile { PrevUrl = "/images/Argo.png" } });

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var user = UserService.GetUserByUserName(AppContext.UserName);
        CurrentUser = new User()
        {
            App = user?.App ?? AppContext.AppId,
            UserName = AppContext.UserName,
            DisplayName = AppContext.DisplayName
        };
        IsDemo = DictService.IsDemo();
        Apps = DictService.GetApps().ToSelectedItemList();
        Themes = DictService.GetThemes().ToSelectedItemList();
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

    private async Task OnSaveDisplayName(EditContext context)
    {
        var ret = UserService.SaveDisplayName(CurrentUser.DisplayName, CurrentUser.UserName);
        await ShowToast(ret, "显示名称");
    }

    private async Task OnSavePassword(EditContext context)
    {
        var ret = UserService.ChangePassword(CurrentUser.UserName, CurrentUser.Password, CurrentUser.NewPassword);
        await ShowToast(ret, "密码");
    }

    private async Task OnSaveApp()
    {
        var ret = UserService.SaveApp(AppContext.UserName, CurrentUser.App);
        await ShowToast(ret, "默认应用");
    }

    private async Task OnSaveTheme()
    {
        var ret = string.IsNullOrEmpty(CurrentUser.Css) ? false : UserService.SaveTheme(AppContext.UserName, CurrentUser.Css);
        await ShowToast(ret, "网站样式");
    }

    private Task OnSaveIcon()
    {
        return Task.CompletedTask;
    }
}
