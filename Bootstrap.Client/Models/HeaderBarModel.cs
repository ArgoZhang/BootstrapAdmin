using Bootstrap.Client.DataAccess;
using Bootstrap.Security;
using Longbow.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Security.Principal;

namespace Bootstrap.Client.Models
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
            var user = BootstrapUser.RetrieveUserByUserName(identity.Name);
            Icon = $"{ConfigurationManager.AppSettings["AuthHost"]}/{user.Icon.TrimStart('~', '/')}";
            DisplayName = user.DisplayName;
            UserName = user.UserName;
            SettingsUrl = DictHelper.RetrieveSettingsUrl();
            ProfilesUrl = DictHelper.RetrieveProfilesUrl();
            var uriBuilder = new UriBuilder(ConfigurationManager.AppSettings["AuthHost"]) { Path = CookieAuthenticationDefaults.LogoutPath };
            LogoutUrl = uriBuilder.ToString();
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
        /// 获得/设置 设置网址
        /// </summary>
        public string SettingsUrl { get; }
        /// <summary>
        /// 获得/设置 个人中心网址
        /// </summary>
        public string ProfilesUrl { get; }
        /// <summary>
        /// 获得 退出登录地址
        /// </summary>
        public string LogoutUrl { get; set; }
    }
}