// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class User
{
    /// <summary>
    /// 获得/设置 系统登录用户名
    /// </summary>
    [Display(Name = "登录名称")]
    [Required(ErrorMessage = "{0}不可为空")]
    [RegularExpression("^[a-zA-Z0-9_@.]*$", ErrorMessage = "登录名称包含非法字符")]
    [MaxLength(16, ErrorMessage = "{0}不能超过 16 个字符")]
    [NotNull]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 用户显示名称
    /// </summary>
    [Display(Name = "显示名称")]
    [Required(ErrorMessage = "{0}不可为空")]
    [MaxLength(20, ErrorMessage = "{0}不能超过 20 个字符")]
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
    [NotNull]
    public string? App { get; set; }

    /// <summary>
    /// 获得/设置 用户主键ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 获取/设置 密码
    /// </summary>
    [Display(Name = "密码")]
    [Required(ErrorMessage = "{0}不可为空")]
    [MaxLength(16, ErrorMessage = "{0}不能超过 16 个字符")]
    [NotNull]
    public string? Password { get; set; }

    /// <summary>
    /// 获取/设置 密码盐
    /// </summary>
    public string? PassSalt { get; set; }

    /// <summary>
    /// 获得/设置 用户注册时间
    /// </summary>
    [Display(Name = "注册时间")]
    public DateTime RegisterTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 获得/设置 用户被批复时间
    /// </summary>
    [Display(Name = "授权时间")]
    public DateTime? ApprovedTime { get; set; }

    /// <summary>
    /// 获得/设置 用户批复人
    /// </summary>
    [Display(Name = "授权人")]
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// 获得/设置 用户的申请理由
    /// </summary>
    [Display(Name = "说明")]
    [NotNull]
    public string? Description { get; set; }

    /// <summary>
    /// 获得/设置 通知描述 2分钟内为刚刚
    /// </summary>
    public string? Period { get; set; }

    /// <summary>
    /// 获得/设置 新密码
    /// </summary>
    [Display(Name = "新密码")]
    [Required(ErrorMessage = "{0}不可为空")]
    [MaxLength(16, ErrorMessage = "{0}不能超过 16 个字符")]
    [NotNull]
    public string? NewPassword { get; set; }

    /// <summary>
    /// 获得/设置 新密码
    /// </summary>
    [Display(Name = "确认密码")]
    [Required(ErrorMessage = "{0}不可为空")]
    [Compare("NewPassword", ErrorMessage = "{0}与{1}不一致")]
    [MaxLength(16, ErrorMessage = "{0}不能超过 16 个字符")]
    [NotNull]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// 获得/设置 是否重置密码
    /// </summary>
    public int IsReset { get; set; }

    /// <summary>
    /// 获得/设置 默认格式为 DisplayName (UserName)
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{DisplayName} ({UserName})";
}

/// <summary>
/// 用户状态枚举类型
/// </summary>
public enum UserStates
{
    /// <summary>
    /// 更改密码
    /// </summary>
    ChangePassword,

    /// <summary>
    /// 更改样式
    /// </summary>
    ChangeTheme,

    /// <summary>
    /// 更改显示名称
    /// </summary>
    ChangeDisplayName,

    /// <summary>
    /// 审批用户
    /// </summary>
    ApproveUser,

    /// <summary>
    /// 拒绝用户
    /// </summary>
    RejectUser,

    /// <summary>
    /// 保存默认应用
    /// </summary>
    SaveApp
}
