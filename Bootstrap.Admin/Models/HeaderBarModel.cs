using Bootstrap.DataAccess;
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
            var user = UserHelper.RetrieveUserByUserName(identity.Name);
            Icon = string.Format("{0}{1}", DictHelper.RetrieveIconFolderPath(), user.Icon);
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            AppCode = user.App;
            if (!string.IsNullOrEmpty(user.Css)) Theme = user.Css;
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
        /// 获得 当前设置的默认应用
        /// </summary>
        public string AppCode { get; }
    }
}