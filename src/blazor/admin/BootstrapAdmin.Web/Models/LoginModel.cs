// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 登陆用户模型
/// </summary>
public class LoginModel
{
    /// <summary>
    /// 获得/设置 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 密码
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// 获得/设置 手机号
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 获得/设置 验证码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 获得/设置 记住我
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// 获得/设置 返回地址
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// 获得/设置 应用 Id
    /// </summary>
    public string? AppId { get; set; }
}
