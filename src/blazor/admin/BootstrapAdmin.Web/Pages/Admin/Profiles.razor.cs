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

    private Task OnSaveDisplayName(EditContext context)
    {
        return Task.CompletedTask;
    }

    private Task OnSavePassword(EditContext context)
    {
        return Task.CompletedTask;
    }

    private Task OnSaveApp()
    {
        if (CurrentUser.App != null)
        {
            UserService.SaveApp(AppContext.UserName, CurrentUser.App);
        }
        return Task.CompletedTask;
    }

    private Task OnSaveTheme()
    {
        return Task.CompletedTask;
    }

    private Task OnSaveIcon()
    {
        return Task.CompletedTask;
    }
}
