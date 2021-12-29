using BootstrapAdmin.Web.Models;
using Longbow.Tasks;

namespace BootstrapAdmin.Web.Extensions;

public static class TasksExtensions
{
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
