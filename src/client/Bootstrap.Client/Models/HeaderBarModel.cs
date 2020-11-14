using Bootstrap.Client.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        /// <param name="controller"></param>
        public HeaderBarModel(ControllerBase controller)
        {
            var user = UserHelper.RetrieveUserByUserName(controller.User.Identity!.Name);
            if (user != null)
            {
                DisplayName = user.DisplayName;
                UserName = user.UserName;
                SettingsUrl = DictHelper.RetrieveSettingsUrl(AppId);
                ProfilesUrl = DictHelper.RetrieveProfilesUrl(AppId);
                NotisUrl = DictHelper.RetrieveNotisUrl(AppId);

                // set LogoutUrl
                var config = controller.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var authHost = config.GetBootstrapAdminAuthenticationOptions().AuthHost;
                var uriBuilder = new UriBuilder(authHost);
                uriBuilder.Path = uriBuilder.Path == "/" ? CookieAuthenticationDefaults.LogoutPath.Value : $"{uriBuilder.Path.TrimEnd('/')}{CookieAuthenticationDefaults.LogoutPath.Value}";
                uriBuilder.Query = $"AppId={AppId}";
                LogoutUrl = uriBuilder.ToString();

                // set Icon
                var icon = $"/{DictHelper.RetrieveIconFolderPath().Trim('~', '/')}/{user.Icon}";
                Icon = user.Icon.Contains("://", StringComparison.OrdinalIgnoreCase) ? user.Icon : (string.IsNullOrEmpty(config.GetValue("SimulateUserName", string.Empty)) ? $"{authHost.TrimEnd('/')}{icon}" : "/images/admin.jpg");
                if (!string.IsNullOrEmpty(user.Css)) Theme = user.Css;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; } = "";

        /// <summary>
        /// 获得/设置 用户头像地址
        /// </summary>
        public string Icon { get; } = "";

        /// <summary>
        /// 获得/设置 设置网址
        /// </summary>
        public string SettingsUrl { get; } = "";

        /// <summary>
        /// 获得/设置 个人中心网址
        /// </summary>
        public string ProfilesUrl { get; } = "";

        /// <summary>
        /// 获得 退出登录地址
        /// </summary>
        public string LogoutUrl { get; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string NotisUrl { get; } = "";
    }
}
