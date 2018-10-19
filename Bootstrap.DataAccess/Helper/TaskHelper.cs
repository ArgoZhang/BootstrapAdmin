using Longbow.Data;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    public static class TaskHelper
    {
        /// <summary>
        /// 查询所有任务
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Task> RetrieveTasks() => DbAdapterManager.Create<Task>().RetrieveTasks();
    }
}
