// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Security.Blazor;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;

namespace BootstrapAdmin.Web.Shared
{
    /// <summary>
    /// MainLayout 布局类
    /// </summary>
    public partial class MainLayout : IDisposable
    {
        private IEnumerable<MenuItem>? MenuItems { get; set; }

        [Inject]
        [NotNull]
        private INavigation? NavigationsService { get; set; }

        [Inject]
        [NotNull]
        private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        [Inject]
        [NotNull]
        private IDict? DictsService { get; set; }

        [Inject]
        [NotNull]
        private IUser? UsersService { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? Context { get; set; }

        [Inject]
        [NotNull]
        private IBootstrapAdminService? SecurityService { get; set; }

        [Inject]
        [NotNull]
        private ITrace? TraceService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        [NotNull]
        private WebClientService? WebClientService { get; set; }

        [Inject]
        [NotNull]
        private NavigationManager? NavigationManager { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? AppContext { get; set; }

        [Inject]
        [NotNull]
        private IIPLocatorProvider? IPLocatorProvider { get; set; }

        private string? Title { get; set; }

        private string? Footer { get; set; }

        private string? UserName { get; set; }

        private bool Lock { get; set; }

        private int LockInterval { get; set; }

        [NotNull]
        private string? Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            AppContext.BaseUri = NavigationManager.ToAbsoluteUri(NavigationManager.BaseUri);
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        }

        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            UserName = state.User.Identity?.Name;

            if (!string.IsNullOrEmpty(UserName))
            {
                var user = UsersService.GetUserByUserName(UserName);
                Context.UserName = UserName;
                Context.DisplayName = user?.DisplayName ?? "未注册账户";
                Icon = string.IsNullOrEmpty(user?.Icon) ? "/images/uploader/default.jpg" : GetIcon(user.Icon);

                MenuItems = NavigationsService.GetAllMenus(UserName).ToMenus();
            }

            Title = DictsService.GetWebTitle();
            Footer = DictsService.GetWebFooter();

            string GetIcon(string icon) => icon.Contains("://", StringComparison.OrdinalIgnoreCase) ? icon : string.Format("{0}{1}", DictsService.GetIconFolderPath(), icon);
            Lock = DictsService.GetAutoLockScreen();
            LockInterval = Convert.ToInt32(DictsService.GetAutoLockScreenInterval());
        }

        private async Task<bool> OnAuthorizing(string url)
        {
            bool ret;
            var relativeUrl = NavigationManager.ToBaseRelativePath(url);
            if (relativeUrl.StartsWith("Account/", StringComparison.OrdinalIgnoreCase))
            {
                ret = true;
            }
            else
            {
                ret = await SecurityService.AuthorizingNavigation(Context.UserName, relativeUrl);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public Task OnUpdateAsync(string key)
        {
            if (key == "title")
            {
                Title = DictsService.GetWebTitle();
            }
            else if (key == "footer")
            {
                Footer = DictsService.GetWebFooter();
            }
            StateHasChanged();
            return Task.CompletedTask;
        }

        private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
        {
            _ = Task.Run(async () =>
            {
                // TODO: 可考虑加入队列中，通过任务管理定时插入提高效率
                var clientInfo = await WebClientService.GetClientInfo();
                var city = "XX XX";
                if (!string.IsNullOrEmpty(clientInfo.Ip))
                {
                    city = await IPLocatorProvider.Locate(clientInfo.Ip) ?? "None";
                }
                TraceService.Log(new Trace
                {
                    Browser = clientInfo.Browser,
                    City = city,
                    Ip = clientInfo.Ip,
                    LogTime = DateTime.Now,
                    OS = clientInfo.OS,
                    UserAgent = clientInfo.UserAgent,
                    RequestUrl = NavigationManager.ToBaseRelativePath(e.Location),
                    UserName = AppContext.UserName
                });
            });
        }

        private Task ReloadMenu()
        {
            MenuItems = NavigationsService.GetAllMenus(Context.UserName).ToMenus();
            StateHasChanged();
            return Task.CompletedTask;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
