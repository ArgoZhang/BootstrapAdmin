using Bootstrap.Admin.Extensions;
using Bootstrap.Admin.Shared;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class TabSetBase : BootstrapComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TabCount { get { return Tabs.Count; } }

        /// <summary>
        /// 
        /// </summary>
        protected List<Tab> Tabs { get; set; } = new List<Tab>();
        private string? curId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (!string.IsNullOrEmpty(curId))
            {
                // Add Tab 后设置 active 状态
                JSRuntime.ActiveMenu(curId);
                curId = "";
            }
        }

        /// <summary>
        /// 添加 Tab 由侧边栏菜单调用
        /// </summary>
        /// <param name="menu"></param>
        public void AddTab(BootstrapMenu menu)
        {
            var tab = Tabs.FirstOrDefault(tb => tb.Id == menu.Id);
            if (tab == null)
            {
                tab = new Tab();
                tab.Init(menu);
                Tabs.Add(tab);
                curId = tab.Id;
                StateHasChanged();
            }
            else
            {
                JSRuntime.ActiveMenu(tab.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public async Task<string?> CloseTab(string? tabId)
        {
            if (!string.IsNullOrEmpty(tabId))
            {
                var tab = Tabs.FirstOrDefault(tb => tb.Id == tabId);
                if (tab != null)
                {
                    tabId = await JSRuntime.RemoveTabAsync(tab.Id);
                    Tabs.Remove(tab);
                    StateHasChanged();
                }
            }
            return tabId;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CloseAllTab()
        {
            NavigationManager?.NavigateTo(RootLayout?.HomeUrl);
        }

        /// <summary>
        /// 
        /// </summary>
        protected async Task MovePrev()
        {
            var url = await JSRuntime.MovePrevTabAsync();
            if (!string.IsNullOrEmpty(url)) NavigationManager?.NavigateTo(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task MoveNext()
        {
            var url = await JSRuntime.MoveNextTabAsync();
            if (!string.IsNullOrEmpty(url)) NavigationManager?.NavigateTo(url);
        }
    }
}
