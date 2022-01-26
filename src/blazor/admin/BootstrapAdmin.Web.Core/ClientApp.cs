// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public class ClientApp
{
    /// <summary>
    /// 获得/设置 AppId 唯一标识
    /// </summary>
    [Display(Name = "应用ID")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string? AppId { get; set; }

    /// <summary>
    /// 获得/设置 App 名称
    /// </summary>
    [Display(Name = "应用名称")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string? AppName { get; set; }

    /// <summary>
    /// 获得/设置 网站首页地址
    /// </summary>
    [Display(Name = "应用首页")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string? HomeUrl { get; set; }

    /// <summary>
    /// 获得/设置 网站 Title
    /// </summary>
    [Display(Name = "网站标题")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string? Title { get; set; }

    /// <summary>
    /// 获得/设置 网站 Footer
    /// </summary>
    [Display(Name = "网站页脚")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string? Footer { get; set; }

    /// <summary>
    /// 获得/设置 App Logo
    /// </summary>
    [Display(Name = "网站图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// 获得/设置 网站图标
    /// </summary>
    [Display(Name = "Favicon")]
    public string? Favicon { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "个人中心地址")]
    public string? ProfileUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "设置地址")]
    public string? SettingsUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "消息通知地址")]
    public string? NotificationUrl { get; set; }
}
