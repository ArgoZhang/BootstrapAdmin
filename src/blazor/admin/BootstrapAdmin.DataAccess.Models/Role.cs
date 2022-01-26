// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// Role 实体类
/// </summary>
public class Role
{
    /// <summary>
    /// 获得/设置 角色主键ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 获得/设置 角色名称
    /// </summary>
    [DisplayName("角色名称")]
    [NotNull]
    public string? RoleName { get; set; }

    /// <summary>
    /// 获得/设置 角色描述
    /// </summary>
    [DisplayName("角色描述")]
    [NotNull]
    public string? Description { get; set; }
}
