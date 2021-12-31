using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using Longbow.Tasks;

namespace BootstrapAdmin.Web.Pages.Admin;

public partial class Tasks
{
    [NotNull]
    private AdminTable<TasksModel>? TaskTable { get; set; }

    private static Task<QueryData<TasksModel>> OnQueryAsync(QueryPageOptions options)
    {
        var tasks = TaskServicesManager.ToList().ToTasksModelList();
        return Task.FromResult(new QueryData<TasksModel>()
        {
            Items = tasks
        });
    }

    private static Color GetResultColor(TriggerResult result) => result switch
    {
        TriggerResult.Success => Color.Success,
        TriggerResult.Error => Color.Danger,
        TriggerResult.Timeout => Color.Warning,
        TriggerResult.Cancelled => Color.Dark,
        _ => Color.Primary
    };

    private static string FormatResult(TriggerResult result) => result switch
    {
        TriggerResult.Success => "成功",
        TriggerResult.Error => "故障",
        TriggerResult.Timeout => "超时",
        TriggerResult.Cancelled => "取消",
        _ => "未知状态"
    };

    private static Color GetStatusColor(SchedulerStatus status) => status switch
    {
        SchedulerStatus.Running => Color.Success,
        SchedulerStatus.Ready => Color.Danger,
        SchedulerStatus.Disabled => Color.Danger,
        _ => Color.Primary
    };

    private static string FormatStatus(SchedulerStatus status) => status switch
    {
        SchedulerStatus.Running => "运行中",
        SchedulerStatus.Ready => "已停止",
        SchedulerStatus.Disabled => "禁用",
        _ => "未知状态"
    };

    private static string GetStatusIcon(SchedulerStatus status) => status switch
    {
        SchedulerStatus.Running => "fa fa-play-circle",
        SchedulerStatus.Ready => "fa fa-times-circle",
        _ => "未知状态"
    };

    private static Task OnPause(TasksModel model)
    {
        var task = TaskServicesManager.ToList().FirstOrDefault(i => i.Name == model.Name);
        if (task != null)
        {
            task.Status = SchedulerStatus.Ready;
        }
        return Task.CompletedTask;
    }

    private static Task OnRun(TasksModel model)
    {
        var task = TaskServicesManager.ToList().FirstOrDefault(i => i.Name == model.Name);
        if (task != null)
        {
            task.Status = SchedulerStatus.Running;
        }
        return Task.CompletedTask;
    }

    private static Task OnLog(TasksModel model)
    {
        return Task.CompletedTask;
    }

    private static bool OnCheckTaskStatus(TasksModel model) => model.Status != SchedulerStatus.Disabled;
}
