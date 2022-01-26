// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IUser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    User? GetUserByUserName(string? userName);

    /// <summary>
    /// 通过用户名获取角色列表
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    List<string> GetRoles(string userName);

    /// <summary>
    /// 通过用户名获得授权 App 集合
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    List<string> GetApps(string userName);

    /// <summary>
    /// 通过用户名获得指定的前台 AppId
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    string? GetAppIdByUserName(string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    List<string> GetUsersByGroupId(string? groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<string> GetUsersByRoleId(string? roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds);

    /// <summary>
    /// 更新密码方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    bool ChangePassword(string userName, string password, string newPassword);

    /// <summary>
    /// 保存显示名称方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    bool SaveDisplayName(string userName, string displayName);

    /// <summary>
    /// 保存用户主题方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="theme"></param>
    /// <returns></returns>
    bool SaveTheme(string userName, string theme);

    /// <summary>
    /// 保存用户头像方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="logo"></param>
    /// <returns></returns>
    bool SaveLogo(string userName, string? logo);

    /// <summary>
    /// 获得所有用户
    /// </summary>
    /// <returns></returns>
    List<User> GetAll();

    /// <summary>
    /// 保存指定用户的默认 App
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="app"></param>
    bool SaveApp(string userName, string app);

    /// <summary>
    /// 认证方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    bool Authenticate(string userName, string password);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    bool SaveUser(string userName, string displayName, string password);
}
