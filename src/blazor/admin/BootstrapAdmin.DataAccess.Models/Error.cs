// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 异常实体类
/// </summary>
public class Error
{
    /// <summary>
    /// 获得/设置 主键
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [DisplayName("应用程序")]
    public string? AppDomainName { get; set; }

    /// <summary>
    /// 获得/设置 用户请求页面地址
    /// </summary>
    [DisplayName("请求网址")]
    public string? ErrorPage { get; set; }

    /// <summary>
    /// 获得/设置 用户 ID
    /// </summary>
    [DisplayName("用户名")]
    public string? UserId { get; set; }

    /// <summary>
    /// 获得/设置 用户 IP
    /// </summary>
    [DisplayName("登录主机")]
    public string? UserIp { get; set; }

    /// <summary>
    /// 获得/设置 异常类型
    /// </summary>
    [DisplayName("异常类型")]
    public string? ExceptionType { get; set; }

    /// <summary>
    /// 获得/设置 异常错误描述信息
    /// </summary>
    [DisplayName("异常描述")]
    public string? Message { get; set; }

    /// <summary>
    /// 获得/设置 异常堆栈信息
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// 获得/设置 日志时间戳
    /// </summary>
    [DisplayName("记录时间")]
    public DateTime LogTime { get; set; }

    /// <summary>
    /// 获得/设置 分类信息
    /// </summary>
    [DisplayName("分类信息")]
    public string? Category { get; set; }
}
