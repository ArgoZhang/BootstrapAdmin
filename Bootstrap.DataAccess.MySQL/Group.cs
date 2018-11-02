using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bootstrap.DataAccess.MySQL
{
    /// <summary>
    /// 
    /// </summary>
    public class Group : DataAccess.Group
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrieveGroups()
        {
            string sql = "select * from `Groups`";
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
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool SaveGroup(DataAccess.Group p)
        {
            bool ret = false;
            if (p.GroupName.Length > 50) p.GroupName = p.GroupName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.Description) && p.Description.Length > 500) p.Description = p.Description.Substring(0, 500);
            string sql = string.IsNullOrEmpty(p.Id) ?
                "Insert Into `Groups` (GroupName, Description) Values (@GroupName, @Description)" :
                "Update `Groups` set GroupName = @GroupName, Description = @Description where ID = @ID";
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
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public override bool DeleteGroup(IEnumerable<string> value)
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

                        cmd.CommandText = $"delete from `Groups` where ID in ({ids})";
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
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrieveGroupsByUserId(string userId)
        {
            string sql = "select g.ID,g.GroupName,g.Description,case ug.GroupID when g.ID then 'checked' else '' end status from `Groups` g left join UserGroup ug on g.ID=ug.GroupID and UserID=@UserID";
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
        public override bool SaveGroupsByUserId(string userId, IEnumerable<string> groupIds)
        {
            var ret = false;

            //判断用户是否选定角色
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    //删除用户部门表中该用户所有的部门关系
                    string sql = $"delete from UserGroup where UserID = {userId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        groupIds.ToList().ForEach(gId =>
                        {
                            cmd.CommandText = $"Insert Into UserGroup (UserID, GroupID) Values ({userId}, {gId})";
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        });
                        transaction.CommitTransaction();
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
        /// <param name="roleId"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Group> RetrieveGroupsByRoleId(string roleId)
        {
            List<Group> groups = new List<Group>();
            string sql = "select g.ID,g.GroupName,g.Description,case rg.GroupID when g.ID then 'checked' else '' end status from `Groups` g left join RoleGroup rg on g.ID=rg.GroupID and RoleID=@RoleID";
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
        public override bool SaveGroupsByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            bool ret = false;
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
                        groupIds.ToList().ForEach(gId =>
                        {
                            cmd.CommandText = $"Insert Into RoleGroup (RoleID, GroupID) Values ({roleId}, {gId})";
                            DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        });
                        transaction.CommitTransaction();
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
    }
}
