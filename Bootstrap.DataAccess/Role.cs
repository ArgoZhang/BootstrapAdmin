using Longbow;
using Longbow.Cache;
using Longbow.Configuration;
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
        public const string RetrieveRolesDataKey = "RoleHelper-RetrieveRoles";
        public const string RetrieveRolesByUserIdDataKey = "RoleHelper-RetrieveRolesByUserId";
        public const string RetrieveRolesByMenuIdDataKey = "RoleHelper-RetrieveRolesByMenuId";
        public const string RetrieveRolesByGroupIdDataKey = "RoleHelper-RetrieveRolesByGroupId";
        public const string RetrieveRolesByUserNameDataKey = "BootstrapAdminRoleMiddleware-RetrieveRolesByUserName";
        public const string RetrieveRolesByUrlDataKey = "BootstrapAdminAuthorizeFilter-RetrieveRolesByUrl";
        /// <summary>
        /// 获得/设置 角色主键ID
        /// </summary>
        public int Id { get; set; }
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
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRoles(int id = 0)
        {
            var ret = CacheManager.GetOrAdd(RetrieveRolesDataKey, key =>
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
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2]
                        });
                    }
                }
                return roles;
            });
            return id == 0 ? ret : ret.Where(t => id == t.Id);
        }
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveRolesByUserId(int userId, IEnumerable<int> roleIds)
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
                    CacheCleanUtility.ClearCache(userIds: new List<int>() { userId }, roleIds: roleIds);
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
        public virtual IEnumerable<Role> RetrieveRolesByUserId(int userId)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByUserIdDataKey, userId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<Role> roles = new List<Role>();
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join UserRole ur on r.ID = ur.RoleID and UserID = @UserID";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@UserID", userId));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return roles;
            }, RetrieveRolesByUserIdDataKey);
        }
        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public virtual bool DeleteRole(IEnumerable<int> value)
        {
            bool ret = false;
            var ids = string.Join(",", value);
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.StoredProcedure, "Proc_DeleteRoles"))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ids", ids));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == -1;
            }
            CacheCleanUtility.ClearCache(roleIds: value);
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
            string sql = p.Id == 0 ?
                "Insert Into Roles (RoleName, Description) Values (@RoleName, @Description)" :
                "Update Roles set RoleName = @RoleName, Description = @Description where ID = @ID";
            using (DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@ID", p.Id));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@RoleName", p.RoleName));
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@Description", DbAdapterManager.ToDBValue(p.Description)));
                ret = DbAccessManager.DBAccess.ExecuteNonQuery(cmd) == 1;
            }
            CacheCleanUtility.ClearCache(roleIds: p.Id == 0 ? new List<int>() : new List<int> { p.Id });
            return ret;
        }
        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRolesByMenuId(int menuId)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByMenuIdDataKey, menuId);
            var ret = CacheManager.GetOrAdd(key, k =>
            {
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join NavigationRole ur on r.ID = ur.RoleID and NavigationID = @NavigationID";
                List<Role> roles = new List<Role>();
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@NavigationID", menuId));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return roles;
            }, RetrieveRolesByMenuIdDataKey);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SavaRolesByMenuId(int menuId, IEnumerable<int> roleIds)
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
                    CacheCleanUtility.ClearCache(roleIds: roleIds, menuIds: new List<int>() { menuId });
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
        public virtual IEnumerable<Role> RetrieveRolesByGroupId(int groupId)
        {
            string key = string.Format("{0}-{1}", RetrieveRolesByGroupIdDataKey, groupId);
            return CacheManager.GetOrAdd(key, k =>
            {
                List<Role> roles = new List<Role>();
                string sql = "select r.ID, r.RoleName, r.[Description], case ur.RoleID when r.ID then 'checked' else '' end [status] from Roles r left join RoleGroup ur on r.ID = ur.RoleID and GroupID = @GroupID";
                DbCommand cmd = DbAccessManager.DBAccess.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(DbAccessManager.DBAccess.CreateParameter("@GroupID", groupId));
                using (DbDataReader reader = DbAccessManager.DBAccess.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            Id = LgbConvert.ReadValue(reader[0], 0),
                            RoleName = (string)reader[1],
                            Description = reader.IsDBNull(2) ? string.Empty : (string)reader[2],
                            Checked = (string)reader[3]
                        });
                    }
                }
                return roles;
            }, RetrieveRolesByGroupIdDataKey);
        }

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveRolesByGroupId(int groupId, IEnumerable<int> roleIds)
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
                    CacheCleanUtility.ClearCache(roleIds: roleIds, groupIds: new List<int>() { groupId });
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
        public virtual IEnumerable<string> RetrieveRolesByUserName(string userName)
        {
            return CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUserNameDataKey, userName), r =>
            {
                var entities = new List<string>();
                var db = DbAccessManager.DBAccess;
                using (DbCommand cmd = db.CreateCommand(CommandType.Text, "select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @UserName union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join Groups g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName=@UserName"))
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
            }, RetrieveRolesByUserNameDataKey);
        }
        /// <summary>
        /// 根据菜单url查询某个所拥有的角色
        /// 从NavigatorRole表查
        /// 从Navigators-〉GroupNavigatorRole-〉Role查查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveRolesByUrl(string url)
        {
            return CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveRolesByUrlDataKey, url), k =>
            {
                string sql = "select distinct r.RoleName, r.[Description] from Roles r inner join NavigationRole nr on r.ID = nr.RoleID inner join Navigations n on nr.NavigationID = n.ID and n.[Application] = @AppId and n.Url like @Url";
                var Roles = new List<string> { "Administrators" };
                var db = DbAccessManager.DBAccess;
                var cmd = db.CreateCommand(CommandType.Text, sql);
                cmd.Parameters.Add(db.CreateParameter("@Url", string.Format("{0}%", url)));
                cmd.Parameters.Add(db.CreateParameter("@AppId", LgbConvert.ReadValue(ConfigurationManager.AppSettings["AppId"], "0")));
                using (DbDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        Roles.Add((string)reader[0]);
                    }
                }
                return Roles;
            }, RetrieveRolesByUrlDataKey);
        }
    }
}
