using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
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
            Navigations = DbHelper.RetrieveAppCascadeMenus(UserName, $"~/{controller.ControllerContext.ActionDescriptor.ControllerName}/{controller.ControllerContext.ActionDescriptor.ActionName}");
            ImageLibUrl = DbHelper.RetrieveImagesLibUrl();
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<BootstrapMenu> Navigations { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ImageLibUrl { get; set; }
    }
}
