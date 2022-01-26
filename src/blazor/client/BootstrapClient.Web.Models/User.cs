// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapClient.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class User
{
    /// <summary>
    /// 获得/设置 系统登录用户名
    /// </summary>
    [Display(Name = "登录名称")]
    [NotNull]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 用户显示名称
    /// </summary>
    [Display(Name = "显示名称")]
    [NotNull]
    public string? DisplayName { get; set; }

    /// <summary>
    /// 获得/设置 用户头像图标路径
    /// </summary>
    [Display(Name = "用户头像")]
    public string? Icon { get; set; }

    /// <summary>
    /// 获得/设置 用户设置样式表名称
    /// </summary>
    [Display(Name = "主题")]
    public string? Css { get; set; }

    /// <summary>
    /// 获得/设置 用户默认登陆 App 标识
    /// </summary>
    [Display(Name = "默认 APP")]
    public string? App { get; set; }

    /// <summary>
    /// 获得/设置 用户主键ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 获得/设置 默认格式为 DisplayName (UserName)
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{DisplayName} ({UserName})";
}
