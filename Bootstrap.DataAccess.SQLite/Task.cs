using Longbow;
using Longbow.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess.SQLite
{
    public class Task : DataAccess.Task
    {
        /// <summary>
        /// 查询所有任务
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Task> RetrieveTasks()
        {
            return CacheManager.GetOrAdd(RetrieveTasksDataKey, key =>
            {
                string sql = "select t.*, u.DisplayName from Tasks t inner join Users u on t.UserName = u.UserName order by AssignTime desc limit 1000";
                List<Task> tasks = new List<Task>();
                DbCommand cmd = DBAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                using (DbDataReader reader = DBAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task()
                        {
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            TaskName = (string)reader[1],
                            AssignName = (string)reader[2],
                            UserName = (string)reader[3],
                            TaskTime = LgbConvert.ReadValue(reader[4], 0),
                            TaskProgress = (double)reader[5],
                            AssignTime = LgbConvert.ReadValue(reader[6], DateTime.MinValue),
                            AssignDisplayName = (string)reader[7]
                        });
                    }
                }
                return tasks;
            });
        }
    }
}
