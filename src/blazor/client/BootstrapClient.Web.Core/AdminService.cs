// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Security.Blazor;

namespace BootstrapClient.Web.Core.Services;

/// <summary>
/// 
/// </summary>
public class AdminService : IBootstrapAdminService
{
    private IUser User { get; set; }

    private INavigation Navigations { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="navigations"></param>
    public AdminService(IUser user, INavigation navigations)
    {
        User = user;
        Navigations = navigations;
    }

    /// <summary>
    /// 通过用户名获取角色集合方法
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetRoles(string userName) => User.GetRoles(userName);

    /// <summary>
    /// 通过用户名获取授权 App 集合方法
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public List<string> GetApps(string userName) => User.GetApps(userName);

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
            ret = Navigations.GetMenus(userName)
                .Any(m => m.Url.Contains(uri.AbsolutePath, StringComparison.OrdinalIgnoreCase));
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
    /// <exception cref="NotImplementedException"></exception>
    public bool AuthorizingBlock(string userName, string url, string blockName)
    {
        // Client 暂时未使用
        return true;
    }
}
