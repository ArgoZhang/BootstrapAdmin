using Longbow.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

public class TasksModel
{
    [Display(Name = "任务名称")]
    [NotNull]
    public string? Name { get; set; }

    [Display(Name = "创建时间")]
    public DateTimeOffset CreateTime { get; set; }

    [Display(Name = "上次运行时间")]
    public DateTimeOffset? LastRuntime { get; set; }

    [Display(Name = "下次运行时间")]
    public DateTimeOffset? NextRuntime { get; set; }

    [Display(Name = "触发条件")]
    [NotNull]
    public string? Trigger { get; set; }

    [Display(Name = "执行结果")]
    [NotNull]
    public TriggerResult LastRunResult { get; set; }

    [Display(Name = "任务状态")]
    [NotNull]
    public SchedulerStatus Status { get; set; }
}
