// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 资源类型枚举 0 表示菜单 1 表示资源 2 表示按钮
/// </summary>
public enum EnumResource
{
    /// <summary>
    /// 
    /// </summary>
    [Description("菜单")]
    Navigation,
    /// <summary>
    /// 
    /// </summary>
    [Description("资源")]
    Resource,
    /// <summary>
    /// 
    /// </summary>
    [Description("代码块")]
    Block
}
