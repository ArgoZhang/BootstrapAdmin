using Longbow.Web;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Pages.Views.Admin.Components
{
    /// <summary>
    /// 网站设置组件
    /// </summary>
    public class OnlineBase : ComponentBase
    {
        /// <summary>
        /// 获得 EditModel 实例
        /// </summary>
        protected OnlineUser EditModel { get; set; } = new OnlineUser();

        /// <summary>
        /// IOnlineUsers 实例
        /// </summary>
        [Inject]
        public IOnlineUsers? OnlineUSers { get; set; }

        /// <summary>
        /// QueryData 方法
        /// </summary>
        protected IEnumerable<OnlineUser> QueryData() => (OnlineUSers?.OnlineUsers ?? new OnlineUser[0]).OrderByDescending(u => u.LastAccessTime);
    }
}
