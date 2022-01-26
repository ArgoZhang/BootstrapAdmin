// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class AppInfo
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "系统名称")]
    [Required(ErrorMessage = "{0}不可为空")]
    [NotNull]
    public string? Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "网站页脚")]
    [Required(ErrorMessage = "{0}不可为空")]
    [NotNull]
    public string? Footer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "登录首页")]
    [NotNull]
    public string? Login { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "后台地址")]
    [NotNull]
    public string? AuthUrl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "网站主题")]
    [NotNull]
    public string? Theme { get; set; }

    /// <summary>
    /// 是否开启默认应用功能
    /// </summary>
    [Display(Name = "默认应用")]
    public bool EnableDefaultApp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "系统演示")]
    public bool IsDemo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "侧边栏设置")]
    public bool SiderbarSetting { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "标题设置")]
    public bool TitleSetting { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "固定表头")]
    public bool FixHeaderSetting { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "健康检查")]
    public bool HealthCheckSetting { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "手机登录")]
    public bool MobileLogin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "OAuth认证")]
    public bool OAuthLogin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "自动锁屏")]
    public bool AutoLock { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "时长间隔（秒）")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int Interval { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "地理位置定位器")]
    public string? IpLocator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "异常日志（月）")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int ExceptionExpired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "操作日志（月）")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int OperateExpired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "操作日志（月）")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int LoginExpired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "访问日志（月）")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int AccessExpired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "Cookie（天）")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int CookieExpired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "IP 缓存（分）")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int IPCacheExpired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "授权码")]
    [Required(ErrorMessage = "{0}不可为空")]
    [NotNull]
    public string? AuthCode { get; set; }
}
