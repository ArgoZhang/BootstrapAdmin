using Longbow.Caching;
using Longbow.Caching.Configuration;
using Longbow.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess
{
    public static class TaskHelper
    {
        private const string RetrieveTasksDataKey = "TaskHelper-RetrieveTasks";
        /// <summary>
        /// 查询所有任务
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Task> RetrieveTasks()
        {
            return CacheManager.GetOrAdd(RetrieveTasksDataKey, CacheSection.RetrieveIntervalByKey(RetrieveTasksDataKey), key =>
            {
                string sql = "select top 1000 t.*, u.DisplayName from Tasks t inner join Users u on t.UserName = u.UserName order by AssignTime desc";
                List<Task> tasks = new List<Task>();
                DbCommand cmd = DBAccessManager.SqlDBAccess.CreateCommand(CommandType.Text, sql);
                try
                {
                    using (DbDataReader reader = DBAccessManager.SqlDBAccess.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task()
                            {
                                Id = (int)reader[0],
                                TaskName = (string)reader[1],
                                AssignName = (string)reader[2],
                                UserName = (string)reader[3],
                                TaskTime = (int)reader[4],
                                TaskProgress = (double)reader[5],
                                AssignTime = (DateTime)reader[6],
                                AssignDisplayName = (string)reader[7]
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return tasks;
            }, CacheSection.RetrieveDescByKey(RetrieveTasksDataKey));
        }
    }
}
