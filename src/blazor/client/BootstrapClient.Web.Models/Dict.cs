// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapClient.DataAccess.Models;

/// <summary>
/// 字典配置项
/// </summary>
public class Dict
{
    /// <summary>
    /// 获得/设置 字典标签
    /// </summary>
    [Display(Name = "字典标签")]
    public string? Category { get; set; }

    /// <summary>
    /// 获得/设置 字典名称
    /// </summary>
    [Display(Name = "字典名称")]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 获得/设置 字典字典值
    /// </summary>
    [Display(Name = "字典代码")]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 获得/设置 字典定义值 0 表示系统使用，1 表示用户自定义 默认为 1
    /// </summary>
    [Display(Name = "字典类型")]
    public EnumDictDefine Define { get; set; } = EnumDictDefine.Customer;
}
