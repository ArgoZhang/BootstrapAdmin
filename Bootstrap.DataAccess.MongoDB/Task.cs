using MongoDB.Driver;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Task : DataAccess.Task
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Task> Retrieves() => DbManager.Tasks.Find(FilterDefinition<DataAccess.Task>.Empty).SortByDescending(task => task.AssignTime).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.Task task)
        {
            DbManager.Tasks.InsertOne(task);
            return true;
        }
    }
}
