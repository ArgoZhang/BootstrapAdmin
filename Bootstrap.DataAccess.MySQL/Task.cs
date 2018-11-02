using Longbow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess.MySQL
{
    public class Task : DataAccess.Task
    {
        /// <summary>
        /// 查询所有任务
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Task> RetrieveTasks()
        {
            string sql = "select t.*, u.DisplayName from Tasks t inner join Users u on t.UserName = u.UserName order by AssignTime desc limit 1000";
            List<DataAccess.Task> tasks = new List<DataAccess.Task>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    tasks.Add(new DataAccess.Task()
                    {
                        Id = reader[0].ToString(),
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
        }
    }
}
