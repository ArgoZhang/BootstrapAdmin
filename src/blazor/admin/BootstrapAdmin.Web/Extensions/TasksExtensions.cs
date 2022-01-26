// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Models;
using Longbow.Tasks;

namespace BootstrapAdmin.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class TasksExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static List<TasksModel> ToTasksModelList(this IEnumerable<IScheduler> schedulers) => schedulers.Select(i => new TasksModel
    {
        Name = i.Name,
        CreateTime = i.CreatedTime,
        LastRuntime = i.LastRuntime,
        NextRuntime = i.NextRuntime,
        LastRunResult = i.LastRunResult,
        Status = i.Status,
        Trigger = i.Triggers.First().ToString()
    }).ToList();
}
