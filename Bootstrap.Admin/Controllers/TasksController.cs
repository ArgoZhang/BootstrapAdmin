using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace Bootstrap.Admin.Controllers
{
    public class TasksController : ApiController
    {
        [HttpGet]
        public Tasks Get()
        {
            var tasks = new Tasks();
            TaskHelper.RetrieveTasks().AsParallel().ForAll(n => tasks.Users.Add(n));
            return tasks;
        }

        public class Tasks
        {
            public Tasks()
            {
                Users = new List<Task>();
            }
            public List<Task> Users { get; set; }
        }
    }
}