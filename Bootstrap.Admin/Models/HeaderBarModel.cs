using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Linq;
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
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            UserID = user.ID;
            HomeUrl = "~/";
            Menus = MenuHelper.RetrieveLinksByUserName(UserName);
            var notis = NotificationHelper.RetrieveNotifications();
            NotifiCount = notis.Count();
            Notifications = notis.Take(6);
            Messages = MessageHelper.RetrieveMessagesHeader(HttpContext.Current.User.Identity.Name);
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
        public bool ShowMenu { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string HomeUrl { get; protected set; }
        /// <summary>
        /// 获得/设置 前台菜单
        /// </summary>
        public IEnumerable<Menu> Menus { get; private set; }
        /// <summary>
        /// 获得/设置 通知内容集合
        /// </summary>
        public IEnumerable<Notification> Notifications { get; set; }
        /// <summary>
        /// 获得/设置 通知数量
        /// </summary>
        public int NotifiCount { get; set; }
        /// <summary>
        /// 或者/设置消息列表
        /// </summary>
        public IEnumerable<Message> Messages { get; set; }

    }
}