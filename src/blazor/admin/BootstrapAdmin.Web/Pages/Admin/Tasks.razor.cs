using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using Longbow.Tasks;

namespace BootstrapAdmin.Web.Pages.Admin;

public partial class Tasks
{
    private Task<QueryData<TasksModel>> OnQueryAsync(QueryPageOptions options)
    {
        var tasks = TaskServicesManager.ToList().ToTasksModelList();
        return Task.FromResult(new QueryData<TasksModel>()
        {
            Items = tasks
        });
    }

    private Task OnPause(TasksModel model)
    {
        return Task.CompletedTask;
    }

    private Task OnRun(TasksModel model)
    {
        return Task.CompletedTask;
    }

    private Task OnLog(TasksModel model)
    {
        return Task.CompletedTask;
    }
}
