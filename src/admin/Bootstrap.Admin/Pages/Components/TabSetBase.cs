using Bootstrap.Admin.Pages.Extensions;
using Bootstrap.Admin.Pages.Shared;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Pages.Components
{
    /// <summary>
    /// TabSet 组件类
    /// </summary>
    public class TabSetBase : ComponentBase
    {
        /// <summary>
        /// 获得 NavigationManager 实例
        /// </summary>
        [Inject]
        protected NavigationManager? NavigationManager { get; set; }

        /// <summary>
        /// 获得 IJSRuntime 实例
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 获得 DefaultLayout 实例
        /// </summary>
        [CascadingParameter(Name = "Default")]
        protected DefaultLayout? RootLayout { get; set; }

        /// <summary>
        /// 获得 页面集合
        /// </summary>
        protected List<PageContentAttributes> Pages { get; set; } = new List<PageContentAttributes>();

        /// <summary>
        /// T获得 ab 集合
        /// </summary>
        protected List<Tab> Tabs { get; set; } = new List<Tab>();
        private string? curId;

        /// <summary>
        /// OnParametersSet 方法
        /// </summary>
        protected override void OnParametersSet()
        {
            var requestUrl = NavigationManager?.Uri ?? "";
            var path = new Uri(requestUrl).PathAndQuery;
            var menus = DataAccess.MenuHelper.RetrieveAllMenus(RootLayout?.UserName);
            var menu = menus.FirstOrDefault(menu => path.Contains(menu.Url.ToBlazorMenuUrl()));
            if (menu != null) AddTab(menu);
        }

        /// <summary>
        /// 页面呈现后回调方法
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
                Tabs.ForEach(t => t.SetActive(false));
                Tabs.Add(tab);
                Pages.ForEach(p => p.Active = false);
                Pages.Add(new PageContentAttributes() { Id = menu.Id, Name = menu.Url.Replace("~/", ""), Active = true });
            }
            else
            {
                Tabs.ForEach(t => t.SetActive(t.Id == tab.Id));
                Pages.ForEach(p => p.Active = p.Id == tab.Id);
            }
            curId = tab.Id;
            StateHasChanged();
        }

        /// <summary>
        /// 关闭指定标签页方法
        /// </summary>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public async Task CloseTab(string? tabId)
        {
            if (Tabs.Count == 1)
            {
                CloseAllTab();
                return;
            }
            if (!string.IsNullOrEmpty(tabId))
            {
                var tab = Tabs.FirstOrDefault(tb => tb.Id == tabId);
                if (tab != null)
                {
                    // 移除 PageContent
                    var page = Pages.FirstOrDefault(p => p.Id == tab.Id);
                    if (page != null) Pages.Remove(page);

                    // 移除 Tab 返回下一个 TabId
                    tabId = await JSRuntime.RemoveTabAsync(tab.Id);
                    Tabs.Remove(tab);
                    tab = Tabs.FirstOrDefault(t => t.Id == tabId);
                    if (tab != null)
                    {
                        tab.SetActive(true);

                        page = Pages.FirstOrDefault(p => p.Id == tabId);
                        if (page != null) page.Active = true;
                        StateHasChanged();
                    }
                }
            }
        }

        /// <summary>
        /// 关闭所有标签页方法
        /// </summary>
        protected void CloseAllTab()
        {
            if (RootLayout != null) NavigationManager?.NavigateTo(RootLayout.HomeUrl);
        }

        /// <summary>
        /// 关闭当前标签页方法
        /// </summary>
        protected async Task CloseCurrentTab()
        {
            var tabId = Tabs.FirstOrDefault(t => t.Active)?.Id;
            await CloseTab(tabId);
        }

        /// <summary>
        /// 关闭其他标签页方法
        /// </summary>
        protected void CloseOtherTab()
        {
            var tabId = Tabs.FirstOrDefault(t => t.Active)?.Id;
            if (!string.IsNullOrEmpty(tabId))
            {
                Tabs.RemoveAll(t => t.Id != tabId);
                Pages.RemoveAll(page => page.Id != tabId);
                curId = tabId;
                StateHasChanged();
            }
        }

        /// <summary>
        /// 前移标签页方法
        /// </summary>
        protected async Task MovePrev()
        {
            var url = await JSRuntime.MovePrevTabAsync();
            if (!string.IsNullOrEmpty(url)) NavigationManager?.NavigateTo(url);
        }

        /// <summary>
        /// 后移标签页方法
        /// </summary>
        /// <returns></returns>
        protected async Task MoveNext()
        {
            var url = await JSRuntime.MoveNextTabAsync();
            if (!string.IsNullOrEmpty(url)) NavigationManager?.NavigateTo(url);
        }
    }
}
