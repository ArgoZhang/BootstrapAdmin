// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

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

    [Inject]
    [NotNull]
    private IWebHostEnvironment? WebHost { get; set; }

    private List<UploadFile> PreviewFileList { get; } = new();

    private string? DefaultLogo { get; set; }

    [NotNull]
    private string? DefaultLogoFolder { get; set; }

    [CascadingParameter]
    [NotNull]
    private Layout? Layout { get; set; }

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
            DisplayName = AppContext.DisplayName,
            Css = user?.Css
        };
        IsDemo = DictService.IsDemo();
        Apps = DictService.GetApps().ToSelectedItemList();
        Themes = DictService.GetThemes().ToSelectedItemList();

        DefaultLogo = DictService.GetDefaultIcon();
        DefaultLogoFolder = DictService.GetIconFolderPath();

        var logoFile = user?.Icon ?? DefaultLogo;
        var logoFolder = DefaultLogoFolder;
        CurrentUser.Icon = Path.Combine(logoFolder, logoFile);
        var fileName = Path.Combine(WebHost.WebRootPath, logoFolder.Replace("/", "\\").TrimStart('\\'), logoFile);
        if (File.Exists(fileName))
        {
            var uploadFile = new UploadFile()
            {
                FileName = logoFile,
                PrevUrl = CurrentUser.Icon
            };
            var fi = new FileInfo(fileName);
            uploadFile.Size = fi.Length;
            PreviewFileList.Add(uploadFile);
        }
    }

    private async Task ShowToast(bool result, string title, string? content = "保存")
    {
        if (result)
        {
            await ToastService.Success(title, $"{content}{title}成功");
        }
        else
        {
            await ToastService.Error(title, $"{content}{title}失败");
        }
    }

    private async Task OnSaveDisplayName(EditContext context)
    {
        var ret = UserService.SaveDisplayName(CurrentUser.UserName, CurrentUser.DisplayName);
        await ShowToast(ret, "显示名称");
        if (ret)
        {
            AppContext.DisplayName = CurrentUser.DisplayName;
            await RenderLayout("displayName");
        }
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

    private async Task OnSaveIcon(UploadFile file)
    {
        // 保存到物理文件
        var logoFile = $"{CurrentUser.UserName}{Path.GetExtension(file.OriginFileName)}";
        var fileName = WebHost.CombineLogoFile(DefaultLogoFolder, logoFile);
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        // 文件大小 10 M
        var ret = await file.SaveToFileAsync(fileName, 10 * 1024 * 1000);

        // 更新用户信息
        if (ret)
        {
            ret = UserService.SaveLogo(CurrentUser.UserName, logoFile);

            CurrentUser.Icon = Path.Combine(DefaultLogoFolder, logoFile);
            PreviewFileList.Clear();
            PreviewFileList.Add(new UploadFile()
            {
                PrevUrl = $"{CurrentUser.Icon}?v={DateTime.Now.Ticks}",
                Size = file.Size,
                FileName = logoFile
            });
        }
        await ShowToast(ret, "用户头像");
    }

    private async Task<bool> OnDeleteIcon(UploadFile file)
    {
        var ret = false;
        var logoFile = file.FileName;
        if (!string.IsNullOrEmpty(logoFile))
        {
            var fileName = WebHost.CombineLogoFile(DefaultLogoFolder, logoFile);
            if (!logoFile.Equals(DefaultLogo, StringComparison.OrdinalIgnoreCase) && File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            ret = UserService.SaveLogo(CurrentUser.UserName, null);
        }
        await ShowToast(ret, "用户头像", "删除");
        return ret;
    }

    private async Task RenderLayout(string key)
    {
        if (Layout.OnUpdateAsync != null)
        {
            await Layout.OnUpdateAsync(key);
        }
    }
}
