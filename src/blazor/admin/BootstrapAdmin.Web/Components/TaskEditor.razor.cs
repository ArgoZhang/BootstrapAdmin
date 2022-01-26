// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class TaskEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public TasksModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<TasksModel> ValueChanged { get; set; }

    private static string TaskName => "测试任务";

    [NotNull]
    private List<SelectedItem>? Items { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Items = new List<SelectedItem>
        {
            new(Longbow.Tasks.Cron.Secondly(5), "每 5 秒钟执行一次"),
            new(Longbow.Tasks.Cron.Minutely(1), "每 1 分钟钟执行一次"),
            new(Longbow.Tasks.Cron.Minutely(5), "每 5 分钟执行一次"),
        };

        if (string.IsNullOrEmpty(Value.Trigger))
        {
            Value.Trigger = Items.First().Value;
        }
    }
}
