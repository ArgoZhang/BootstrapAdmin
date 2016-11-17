using Longbow;
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
        internal const string RetrieveTasksDataKey = "TaskHelper-RetrieveTasks";
        /// <summary>
        /// 查询所有任务
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Task> RetrieveTasks()
        {
            var ret = CacheManager.GetOrAdd(RetrieveTasksDataKey, CacheSection.RetrieveIntervalByKey(RetrieveTasksDataKey), key =>
            {
                string sql = "select * from Tasks";
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
                                ID = (int)reader[0],
                                TaskName = (string)reader[1],
                                AssignName = (string)reader[2],
                                UserName = (string)reader[3],
                                TaskTime = (int)reader[4],
                                TaskProgress = (double)reader[5],
                                AssignTime = LgbConvert.ReadValue(reader[6], DateTime.MinValue)
                            });
                        }
                    }
                }
                catch (Exception ex) { ExceptionManager.Publish(ex); }
                return tasks;
            }, CacheSection.RetrieveDescByKey(RetrieveTasksDataKey));
            return ret;
        }
    }
}
