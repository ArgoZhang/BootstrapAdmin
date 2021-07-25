using Bootstrap.Client.DataAccess;
using Bootstrap.Security;
using Bootstrap.Security.Mvc;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Client.Blazor.Shared.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MainLayout
    {
        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        [NotNull]
        private List<MenuItem>? Menus { get; set; }

        [NotNull]
        private Dictionary<string, string>? TabItemTextDictionary { get; set; }

        /// <summary>
        /// 获得 当前用户登录显示名称
        /// </summary>
        [NotNull]
        public string? DisplayName { get; private set; }

        /// <summary>
        /// 获得 当前用户登录名
        /// </summary>
        [NotNull]
        public string? UserName { get; private set; }

        [Inject]
        [NotNull]
        private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // 通过当前登录名获取显示名称
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated ?? false)
            {
                UserName = state.User.Identity.Name;
                DisplayName = UserHelper.RetrieveUserByUserName(UserName)?.DisplayName;

                // 模拟异步线程切换，正式代码中删除此行代码
                await Task.Yield();

                // 菜单获取可以通过数据库获取，此处为示例直接拼装的菜单集合
                TabItemTextDictionary = new()
                {
                    [""] = "Index"
                };

                // 获取登录用户菜单
                Menus = GetMenus();
            }
        }

        private List<MenuItem> GetMenus()
        {
            var menus = new List<MenuItem>();

            if (OperatingSystem.IsBrowser())
            {
                // 需要调用 webapi 获取菜单数据 暂未实现
            }
            else
            {
                var data = MenuHelper.RetrieveAppMenus(UserName, "");
                menus = CascadeMenu(data);
            }
            return menus;
        }

        private List<MenuItem> CascadeMenu(IEnumerable<BootstrapMenu> datasource)
        {
            var menus = new List<MenuItem>();
            foreach (var m in datasource)
            {
                var item = new MenuItem()
                {
                    Text = m.Name,
                    Url = m.Url.TrimStart('~'),
                    Target = m.Target,
                    Icon = m.Icon
                };
                menus.Add(item);

                if (m.Menus.Any())
                {
                    item.Items = CascadeMenu(m.Menus);
                }
            }
            return menus;
        }
    }
}
