using Bootstrap.Security.DataAccess;
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
    public class Role
    {
        /// <summary>
        /// 获得/设置 角色主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获得/设置 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 获得/设置 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取/设置 用户角色关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; }

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRoles()
        {
            string sql = "select * from Roles";
            var roles = new List<Role>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    roles.Add(new Role()
                    {
                        Id = reader[0].ToString(),
                        RoleName = (string)reader[1],
                        Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2]
                    });
                }
            }
            return roles;
        }

        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveRolesByUserId(string userId, IEnumerable<string> roleIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            //判断用户是否选定角色
            roleIds.ToList().ForEach(roleId => dt.Rows.Add(userId, roleId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = $"delete from UserRole where UserID = {userId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);
                        if (dt.Rows.Count > 0)
                        {
                            // insert batch data into config table
                            using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                            {
                                bulk.DestinationTableName = "UserRole";
                                bulk.ColumnMappings.Add("UserID", "UserID");
                                bulk.ColumnMappings.Add("RoleID", "RoleID");
                                bulk.WriteToServer(dt);
                            }
                        }
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
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRolesByUserId(string userId)
        {
            List<Role> roles = new List<Role>();
            string sql = "select r.ID, r.RoleName, r.Description, case ur.RoleID when r.ID then 'checked' else '' end status from Roles r left join UserRole ur on r.ID = ur.RoleID and UserID = @UserID";
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserID", userId));
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    roles.Add(new Role()
                    {
                        Id = reader[0].ToString(),
                        RoleName = (string)reader[1],
                        Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                        Checked = (string)reader[3]
                    });
                }
            }
            return roles;
        }

        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public virtual bool DeleteRole(IEnumerable<string> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, $"delete from UserRole where RoleID in ({ids})"))
                {
                    try
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from RoleGroup where RoleID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from NavigationRole where RoleID in ({ids})";
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        cmd.CommandText = $"delete from Roles where ID in ({ids})";
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
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveRole(Role p)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(p.RoleName) && p.RoleName.Length > 50) p.RoleName = p.RoleName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.Description) && p.Description.Length > 50) p.Description = p.Description.Substring(0, 500);
            string sql = string.IsNullOrEmpty(p.Id) ?
                "Insert Into Roles (RoleName, Description) Values (@RoleName, @Description)" :
                "Update Roles set RoleName = @RoleName, Description = @Description where ID = @ID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleName", p.RoleName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Description", DbAdapterManager.ToDBValue(p.Description)));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            return ret;
        }

        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRolesByMenuId(string menuId)
        {
            string sql = "select r.ID, r.RoleName, r.Description, case ur.RoleID when r.ID then 'checked' else '' end status from Roles r left join NavigationRole ur on r.ID = ur.RoleID and NavigationID = @NavigationID";
            List<Role> roles = new List<Role>();
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@NavigationID", menuId));
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    roles.Add(new Role()
                    {
                        Id = reader[0].ToString(),
                        RoleName = (string)reader[1],
                        Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                        Checked = (string)reader[3]
                    });
                }
            }
            return roles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SavaRolesByMenuId(string menuId, IEnumerable<string> roleIds)
        {
            var ret = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("NavigationID", typeof(int));
            dt.Columns.Add("RoleID", typeof(int));
            //判断用户是否选定角色
            roleIds.ToList().ForEach(roleId => dt.Rows.Add(menuId, roleId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete role from config table
                    string sql = $"delete from NavigationRole where NavigationID = {menuId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "NavigationRole";
                            bulk.ColumnMappings.Add("NavigationID", "NavigationID");
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
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
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRolesByGroupId(string groupId)
        {
            List<Role> roles = new List<Role>();
            string sql = "select r.ID, r.RoleName, r.Description, case ur.RoleID when r.ID then 'checked' else '' end status from Roles r left join RoleGroup ur on r.ID = ur.RoleID and GroupID = @GroupID";
            DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
            cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@GroupID", groupId));
            using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    roles.Add(new Role()
                    {
                        Id = reader[0].ToString(),
                        RoleName = (string)reader[1],
                        Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                        Checked = (string)reader[3]
                    });
                }
            }
            return roles;
        }

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveRolesByGroupId(string groupId, IEnumerable<string> roleIds)
        {
            var ret = false;
            //构造表格
            DataTable dt = new DataTable();
            dt.Columns.Add("RoleID", typeof(int));
            dt.Columns.Add("GroupID", typeof(int));
            roleIds.ToList().ForEach(roleId => dt.Rows.Add(roleId, groupId));
            using (TransactionPackage transaction = DbAccessManager.DBAccess.BeginTransaction())
            {
                try
                {
                    // delete user from config table
                    string sql = $"delete from RoleGroup where GroupID = {groupId}";
                    using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
                    {
                        DbAccessManager.DBAccess.ExecuteNonQuery(cmd, transaction);

                        // insert batch data into config table
                        using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)transaction.Transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.Transaction))
                        {
                            bulk.BatchSize = 1000;
                            bulk.DestinationTableName = "RoleGroup";
                            bulk.ColumnMappings.Add("RoleID", "RoleID");
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
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveRolesByUserName(string userName) => DbHelper.RetrieveRolesByUserName(userName);

        /// <summary>
        /// 根据菜单url查询某个所拥有的角色
        /// 从NavigatorRole表查
        /// 从Navigators-〉GroupNavigatorRole-〉Role查查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveRolesByUrl(string url) => DbHelper.RetrieveRolesByUrl(url);
    }
}
