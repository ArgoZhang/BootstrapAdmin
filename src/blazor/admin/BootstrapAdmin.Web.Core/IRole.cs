// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IRole
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Role> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    List<string> GetRolesByGroupId(string? groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    List<string> GetRolesByUserId(string? userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    List<string> GetRolesByMenuId(string? menuId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds);
}
