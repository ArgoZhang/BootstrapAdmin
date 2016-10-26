using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class NavigatorBarModel : HeaderBarModel
    {
        public NavigatorBarModel()
        {
            Menus = MenuHelper.RetrieveMenus().ToList();
            Menus.ForEach(m => m.Active = null);
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Menu> Menus { get; set; }
    }
}