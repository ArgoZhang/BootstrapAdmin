using Bootstrap.DataAccess;
using System;
using System.Security.Principal;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderBarModel : ModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        public HeaderBarModel(IIdentity identity)
        {
            var user = UserHelper.RetrieveUserByUserName(identity);
            if (user != null)
            {
                Icon = user.Icon.Contains("://", StringComparison.OrdinalIgnoreCase) ? user.Icon : string.Format("{0}{1}", DictHelper.RetrieveIconFolderPath(), user.Icon);
                DisplayName = user.DisplayName;
                UserName = user.UserName;
                AppCode = user.App;
                Css = user.Css;
                ActiveCss = string.IsNullOrEmpty(Css) ? Theme : Css;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 获得/设置 用户头像地址
        /// </summary>
        public string Icon { get; }

        /// <summary>
        /// 获取/设置 个人网站样式
        /// </summary>
        public string Css { get; }

        /// <summary>
        /// 获得 当前设置的默认应用
        /// </summary>
        public string AppCode { get; }

        /// <summary>
        /// 获得 当前样式
        /// </summary>
        public string ActiveCss { get; }
    }
}
