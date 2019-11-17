using Longbow.Cache;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveTasksDataKey = "TaskHelper-RetrieveTasks";

        /// <summary>
        /// 查询所有任务
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Task> Retrieves() => CacheManager.GetOrAdd(RetrieveTasksDataKey, key => DbContextManager.Create<Task>()?.Retrieves()) ?? new Task[0];

        /// <summary>
        /// 保存任务方法
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static bool Save(Task task) => DbContextManager.Create<Task>()?.Save(task) ?? false;
    }
}
