using Bootstrap.DataAccess;
using System.Collections.Generic;
using System.Web.Http;

namespace Bootstrap.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TasksController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return TaskHelper.RetrieveTasks();
        }
    }
}