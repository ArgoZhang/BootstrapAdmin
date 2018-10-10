using Bootstrap.Security;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bootstrap.Client.Models
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
            Navigations = BootstrapMenu.RetrieveAppMenus(UserName, $"~/{controller.ControllerContext.ActionDescriptor.ControllerName}/{controller.ControllerContext.ActionDescriptor.ActionName}");
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<BootstrapMenu> Navigations { get;}
    }
}