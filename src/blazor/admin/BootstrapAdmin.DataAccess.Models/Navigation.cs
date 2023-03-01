// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// Bootstrap Admin 后台管理菜单相关操作实体类
/// </summary>
public class Navigation
{
    /// <summary>
    /// 获得/设置 菜单主键ID
    /// </summary>
    [NotNull]
    [Key]
    public string? Id { set; get; }

    /// <summary>
    /// 获得/设置 父级菜单ID 默认为 0
    /// </summary>
    public string ParentId { set; get; } = "0";

    /// <summary>
    /// 获得/设置 菜单名称
    /// </summary>
    [Display(Name = "名称")]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 获得/设置 菜单序号
    /// </summary>
    [Display(Name = "序号")]
    public int Order { get; set; }

    /// <summary>
    /// 获得/设置 菜单图标
    /// </summary>
    [Display(Name = "图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// 获得/设置 菜单URL地址
    /// </summary>
    [NotNull]
    [Display(Name = "地址")]
    public string? Url { get; set; }

    /// <summary>
    /// 获得/设置 菜单分类, 0 表示系统菜单 1 表示用户自定义菜单
    /// </summary>
    [Display(Name = "类别")]
    public EnumNavigationCategory Category { get; set; }

    /// <summary>
    /// 获得/设置 链接目标
    /// </summary>
    [Display(Name = "目标")]
    public string? Target { get; set; }

    /// <summary>
    /// 获得/设置 是否为资源文件, 0 表示菜单 1 表示资源 2 表示按钮
    /// </summary>
    [Display(Name = "类型")]
    public EnumResource IsResource { get; set; }

    /// <summary>
    /// 获得/设置 所属应用程序，此属性由BA后台字典表分配
    /// </summary>
    [Display(Name = "所属应用")]
    public string? Application { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasChildren { get; set; }
}
