using Bootstrap.Admin.Extensions;
using Bootstrap.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class PageSetBase : BootstrapComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected List<PageContentAttributes> Pages { get; set; } = new List<PageContentAttributes>();
        private string? curId = "";

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        protected IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (!string.IsNullOrEmpty(curId))
            {
                // Add Page 后设置 active 状态
                Activate(curId);
                curId = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        public void Activate(string? pageId) => JSRuntime.ActivePage(pageId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        public void AddPage(BootstrapMenu menu)
        {
            var page = Pages.FirstOrDefault(p => p.Id == menu.Id);
            if (page == null)
            {
                Pages.Add(new PageContentAttributes() { Id = menu.Id, Name = menu.Url });
                curId = menu.Id;
                StateHasChanged();
            }
            else
            {
                Activate(menu.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="newId"></param>
        public void RemovePage(string? pageId, string? newId)
        {
            if (!string.IsNullOrEmpty(pageId))
            {
                var page = Pages.FirstOrDefault(p => p.Id == pageId);
                if (page != null)
                {
                    Pages.Remove(page);
                    curId = newId;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAllPage()
        {
            Pages.Clear();
        }
    }
}
