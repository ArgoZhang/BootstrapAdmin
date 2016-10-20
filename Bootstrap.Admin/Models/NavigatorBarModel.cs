using Bootstrap.DataAccess;
using System.Collections.Generic;

namespace Bootstrap.Admin.Models
{
    public class NavigatorBarModel
    {
        public NavigatorBarModel()
        {
            Navigator = Menu.RetrieveMenus();
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Menu> Navigator { get; set; }
    }
}