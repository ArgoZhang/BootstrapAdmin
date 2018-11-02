using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 获得/设置 群组主键ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 获得/设置 群组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 获得/设置 群组描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 获取/设置 用户群组关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; }
        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IEnumerable<Group> RetrieveGroups()
        {
            string sql = "select * from Groups";
            List<Group> groups = new List<Group>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    groups.Add(new Group()
                    {
                        Id = reader[0].ToString(),
                        GroupName = (string)reader[1],
                        Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2]
                    });
                }
            }
            return groups;
        }
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public virtual bool DeleteGroup(IEnumerable<string> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, $"delete from UserGroup where GroupID in ({ids})"))
                {
                    try
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from RoleGroup where GroupID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from Groups where ID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        transaction.CommitTransaction();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.RollbackTransaction();
                        throw ex;
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 保存新建/更新的群组信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveGroup(Group p)
        {
            bool ret = false;
            if (p.GroupName.Length > 50) p.GroupName = p.GroupName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.Description) && p.Description.Length > 500) p.Description = p.Description.Substring(0, 500);
            string sql = string.IsNullOrEmpty(p.Id) ?
                "Insert Into Groups (GroupName, Description) Values (@GroupName, @Description)" :
                "Update Groups set GroupName = @GroupName, Description = @Description where ID = @ID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@GroupName", p.GroupName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Description", DbAdapterManager.ToDBValue(p.Description)));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            return ret;
        }
        /// <summary>
        /// 根据用户查询部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Group> RetrieveGroupsByUserId(string userId)
        {
            string sql = "select g.ID,g.GroupName,g.Description,case ug.GroupID when g.ID then 'checked' else '' end status from Groups g left join UserGroup ug on g.ID=ug.GroupID and UserID=@UserID";
            List<Group> groups = new List<Group>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserID", userId));
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    groups.Add(new Group()
                    {
                        Id = reader[0].ToString(),
                        GroupName = (string)reader[1],
                        Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                        Checked = (string)reader[3]
                    });
                }
            }
            return groups;
        }
        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public virtual bool SaveGroupsByUserId(string userId, IEnumerable<string> groupIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            //判断用户是否选定角色
            groupIds.ToList().ForEach(groupId => dt.Rows.Add(userId, groupId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户部门表中该用户所有的部门关系
                    string sql = $"delete from UserGroup where UserID = {userId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "UserGroup";
                            bulk.ColumnMappings.Add("UserID", "UserID");
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    ret = true;
                }
                catch (Exception ex)
                {
                    transaction.RollbackTransaction();
                    throw ex;
                }
            }
            return ret;
        }
        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Group> RetrieveGroupsByRoleId(string roleId)
        {
            List<Group> groups = new List<Group>();
            string sql = "select g.ID,g.GroupName,g.Description,case rg.GroupID when g.ID then 'checked' else '' end status from Groups g left join RoleGroup rg on g.ID=rg.GroupID and RoleID=@RoleID";
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleID", roleId));
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    groups.Add(new Group()
                    {
                        Id = reader[0].ToString(),
                        GroupName = (string)reader[1],
                        Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                        Checked = (string)reader[3]
                    });
                }
            }
            return groups;
        }
        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public virtual bool SaveGroupsByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            bool ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("GroupID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            groupIds.ToList().ForEach(groupId => dt.Rows.Add(groupId, roleId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除角色部门表该角色所有的部门
                    string sql = $"delete from RoleGroup where RoleID = {roleId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        //批插入角色部门表
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.ColumnMappings.Add("GroupID", "GroupID");
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
                            bulk.DestinationTableName = "RoleGroup";
                            bulk.WriteToServer(dt);
                            transaction.CommitTransaction();
                        }
                    }
                    ret = true;
                }
                catch (Exception ex)
                {
                    transaction.RollbackTransaction();
                    throw ex;
                }
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveGroupsByUserName(string userName)
        {
            var entities = new List<string>();
            var db = DbAccessManager.DBAccess;
            using (DbCommand cmd = db.CreateCommand(CommandType.Text, "select g.GroupName, g.Description from Groups g inner join UserGroup ug on g.ID = ug.GroupID inner join Users u on ug.UserID = u.ID where UserName = @UserName"))
            {
                cmd.Parameters.Add(db.CreateParameter("@UserName", userName));
                using (DbDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        entities.Add((string)reader[0]);
                    }
                }
            }
            return entities;
        }
    }
}
