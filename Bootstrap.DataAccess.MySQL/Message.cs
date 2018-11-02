using Longbow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess.MySQL
{
    /// <summary>
    /// 
    /// </summary>
    public class Message : DataAccess.Message
    {
        /// <summary>
        /// 所有有关userName所有消息列表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected override IEnumerable<DataAccess.Message> RetrieveMessages(string userName)
        {
            string sql = "select m.*, d.Name, ifnull(i.Code + u.Icon, '~/images/uploader/default.jpg'), u.DisplayName from Messages m left join Dicts d on m.Label = d.Code and d.Category = '消息标签' and d.Define = 0 left join Dicts i on i.Category = '头像地址' and i.Name = '头像路径' and i.Define = 0 inner join Users u on m.`From` = u.UserName where `To` = @UserName or `From` = @UserName order by m.SendTime desc";
            List<DataAccess.Message> messages = new List<DataAccess.Message>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserName", userName));
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    messages.Add(new DataAccess.Message()
                    {
                        Id = reader[0].ToString(),
                        Title = (string)reader[1],
                        Content = (string)reader[2],
                        From = (string)reader[3],
                        To = (string)reader[4],
                        SendTime = LgbConvert.ReadValue(reader[5], DateTime.MinValue),
                        Status = (string)reader[6],
                        Mark = LgbConvert.ReadValue(reader[7], 0),
                        IsDelete = LgbConvert.ReadValue(reader[8], 0),
                        Label = (string)reader[9],
                        LabelName = LgbConvert.ReadValue(reader[10], string.Empty),
                        FromIcon = (string)reader[11],
                        FromDisplayName = (string)reader[12]
                    });
                }
            }
            return messages;
        }
    }
}
