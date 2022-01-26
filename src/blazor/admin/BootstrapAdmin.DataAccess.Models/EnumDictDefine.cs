// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 字典定义值 0 表示系统使用，1 表示用户自定义 默认为 1
/// </summary>
public enum EnumDictDefine
{
    /// <summary>
    /// 系统使用
    /// </summary>
    [Description("系统使用")]
    System,

    /// <summary>
    /// 用户自定义
    /// </summary>
    [Description("自定义")]
    Customer
}
