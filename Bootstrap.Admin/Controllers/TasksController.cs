using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    public class TasksController : ApiController
    {
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return TaskHelper.RetrieveTasks();
        }
    }
}