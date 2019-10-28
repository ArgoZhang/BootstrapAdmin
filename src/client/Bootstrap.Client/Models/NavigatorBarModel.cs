using Bootstrap.Client.DataAccess;
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
        public NavigatorBarModel(ControllerBase controller) : base(controller)
        {
            Navigations = MenuHelper.RetrieveAppMenus(UserName, $"~/{controller.ControllerContext.ActionDescriptor.ControllerName}/{controller.ControllerContext.ActionDescriptor.ActionName}");
            ImageLibUrl = DictHelper.RetrieveImagesLibUrl();
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
