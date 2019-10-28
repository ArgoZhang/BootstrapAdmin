using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 侧边栏导航条 Model
    /// </summary>
    public class NavigatorBarModel : HeaderBarModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="controller"></param>
        public NavigatorBarModel(ControllerBase controller) : base(controller.User.Identity.Name)
        {
            Navigations = MenuHelper.RetrieveSystemMenus(UserName, $"~{controller.HttpContext.Request.Path}");
            var authApps = AppHelper.RetrievesByUserName(controller.User.Identity.Name);
            Applications = DictHelper.RetrieveApps().Where(app => app.Key.IsNullOrEmpty() || authApps.Any(key => key.Equals(app.Key, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// 获得 网站菜单
        /// </summary>
        public IEnumerable<BootstrapMenu> Navigations { get; private set; }

        /// <summary>
        /// 获得 网站应用程序
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Applications { get; private set; }
    }
}
