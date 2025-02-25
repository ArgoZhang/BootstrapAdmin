// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Security.Blazor;

namespace BootstrapClient.Web.Core.Services;

/// <summary>
/// 
/// </summary>
/// <param name="user"></param>
/// <param name="navigations"></param>
public class AdminService(IUser user, INavigation navigations) : IBootstrapAdminService
{

    /// <summary>
    /// 通过用户名获取角色集合方法
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetRoles(string userName) => user.GetRoles(userName);

    /// <summary>
    /// 通过用户名获取授权 App 集合方法
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetApps(string userName) => user.GetApps(userName);

    /// <summary>
    /// 通过用户名检查当前请求 Url 是否已授权方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<bool> AuthorizingNavigation(string userName, string url)
    {
        var ret = false;
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
        {
            ret = navigations.GetMenus(userName)
                .Any(m => m.Url?.Contains(uri.AbsolutePath, StringComparison.OrdinalIgnoreCase) ?? false);
        }
        return Task.FromResult(ret);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="url"></param>
    /// <param name="blockName"></param>
    /// <returns></returns>
    public bool AuthorizingBlock(string userName, string url, string blockName)
    {
        var ret = user.GetRoles(userName).Any(i => i.Equals("Administrators", StringComparison.OrdinalIgnoreCase));
        if (!ret)
        {
            var menus = navigations.GetMenus(userName);
            var menu = menus.FirstOrDefault(m => m.Url.Contains(url, StringComparison.OrdinalIgnoreCase));
            if (menu != null)
            {
                ret = menus.Any(m => m.ParentId == menu.Id && m.IsResource == DataAccess.Models.EnumResource.Block && m.Url.Equals(blockName, StringComparison.OrdinalIgnoreCase));
            }
        }
        return ret;
    }
}
