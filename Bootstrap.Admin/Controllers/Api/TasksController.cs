using Longbow.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<object> Get()
        {
            return TaskServicesManager.ToList().Select(s => new { s.Name, Status = s.Status.ToString(), s.LastRuntime, s.CreatedTime, s.NextRuntime, LastRunResult = s.Triggers.First().LastResult.ToString(), TriggerExpression = s.Triggers.FirstOrDefault().ToString() }).OrderBy(s => s.Name);
        }
    }
}
