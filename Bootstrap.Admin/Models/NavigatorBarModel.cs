using Bootstrap.DataAccess;
using Bootstrap.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigatorBarModel : HeaderBarModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public NavigatorBarModel(ControllerBase controller) : base(controller.User.Identity)
        {
            Navigations = MenuHelper.RetrieveSystemMenus(UserName, $"~{controller.HttpContext.Request.Path}");
            var authApps = AppHelper.RetrievesByUserName(controller.User.Identity.Name);
            Applications = DictHelper.RetrieveApps().Where(app => app.Key == "0" || authApps.Any(key => key.Equals(app.Key, StringComparison.OrdinalIgnoreCase)));
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<BootstrapMenu> Navigations { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Applications { get; private set; }
    }
}