using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Web;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderBarModel : ModelBase
    {
        public HeaderBarModel()
        {
            var user = UserHelper.RetrieveUsersByName(HttpContext.Current.User.Identity.Name);
            Icon = user.Icon;
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            UserID = user.ID;
            HomeUrl = "~/";
            Menus = MenuHelper.RetrieveLinksByUserName(UserName);
        }
        public string UserName { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int UserID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowMenu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HomeUrl { get; protected set; }
        /// <summary>
        /// 获得/设置 前台菜单
        /// </summary>
        public IEnumerable<Menu> Menus { get; private set; }
        /// <summary>
        /// 获得/设置 用户头像地址
        /// </summary>
        public string Icon { get; set; }
    }
}