using Longbow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess
{
    public class Task
    {
        /// <summary>
        /// 获取/设置  任务ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 获取/设置  任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 获取/设置  分配人
        /// </summary>
        public string AssignName { get; set; }
        /// <summary>
        /// 获得/设置 分配人昵称
        /// </summary>
        public string AssignDisplayName { get; set; }
        /// <summary>
        /// 获取/设置  完成任务人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 获取/设置  任务所需时间（天）
        /// </summary>
        public int TaskTime { get; set; }
        /// <summary>
        /// 获取/设置  任务进度
        /// </summary>
        public double TaskProgress { get; set; }
        /// <summary>
        /// 获取/设置  分配时间
        /// </summary>
        public DateTime AssignTime { get; set; }
        /// <summary>
        /// 查询所有任务
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Task> RetrieveTasks()
        {
            string sql = "select top 100 t.*, u.DisplayName from Tasks t inner join Users u on t.UserName = u.UserName order by AssignTime desc";
            List<Task> tasks = new List<Task>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    tasks.Add(new Task()
                    {
                        Id = reader[0].ToString(),
                        TaskName = (string)reader[1],
                        AssignName = (string)reader[2],
                        UserName = (string)reader[3],
                        TaskTime = (int)reader[4],
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
