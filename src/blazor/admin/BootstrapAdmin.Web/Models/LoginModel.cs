// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 登陆用户模型
/// </summary>
public class LoginModel
{
    /// <summary>
    /// 获得/设置 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不可为空")]
    [NotNull]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 密码
    /// </summary>
    [Required(ErrorMessage = "密码不可为空")]
    [NotNull]
    public string? Password { get; set; }

    /// <summary>
    /// 获得/设置 记住我
    /// </summary>
    public bool RememberMe { get; set; }
}
