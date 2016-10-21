using Bootstrap.DataAccess;
using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    public class NavigatorBarModel : HeaderBarModel
    {
        public NavigatorBarModel()
        {
            Menus = Menu.RetrieveMenus();
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Menu> Menus { get; set; }
    }
}