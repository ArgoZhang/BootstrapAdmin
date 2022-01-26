// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 
/// </summary>
public class TasksModel
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "任务名称")]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "创建时间")]
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "上次运行时间")]
    public DateTimeOffset? LastRuntime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "下次运行时间")]
    public DateTimeOffset? NextRuntime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "触发条件")]
    [NotNull]
    public string? Trigger { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "执行结果")]
    [NotNull]
    public TriggerResult LastRunResult { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "任务状态")]
    [NotNull]
    public SchedulerStatus Status { get; set; }
}
