// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 后台数据库脚本执行日志实体类
/// </summary>
public class DBLog
{
    /// <summary>
    /// 获得/设置 主键ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 获得/设置 当前登陆名
    /// </summary>
    [DisplayName("所属用户")]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 数据库执行脚本
    /// </summary>
    [DisplayName("脚本内容")]
    public string SQL { get; set; } = "";

    /// <summary>
    /// 获取/设置 用户角色关联状态 checked 标示已经关联 '' 标示未关联
    /// </summary>
    [DisplayName("执行时间")]
    public DateTime LogTime { get; set; }
}
