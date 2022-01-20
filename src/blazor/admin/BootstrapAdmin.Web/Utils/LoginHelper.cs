using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Utils;

/// <summary>
/// 登录获取默认首页帮助类
/// </summary>
public static class LoginHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="returnUrl"></param>
    /// <param name="appId"></param>
    /// <param name="defaultAppId"></param>
    /// <param name="userService"></param>
    /// <param name="dictService"></param>
    /// <returns></returns>
    public static string GetDefaultUrl(string userName, string? returnUrl, string? appId, string defaultAppId, IUser userService, IDict dictService)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            // 查找 User 设置的默认应用
            appId ??= userService.GetAppIdByUserName(userName) ?? defaultAppId;

            if (appId == defaultAppId && dictService.GetEnableDefaultApp())
            {
                // 开启默认应用
                appId = dictService.GetApps().FirstOrDefault(d => d.Key != defaultAppId).Key;
            }

            if (!string.IsNullOrEmpty(appId))
            {
                returnUrl = dictService.GetHomeUrlByAppId(appId);
            }
        }

        return returnUrl ?? "/Admin/Index";
    }
}
