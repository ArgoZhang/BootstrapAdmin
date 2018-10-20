using Longbow;
using Longbow.Cache;
using Longbow.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

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
        public virtual IEnumerable<Role> RetrieveRoles(int id = 0) => throw new NotImplementedException();
        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveRolesByUserId(int id, IEnumerable<int> roleIds) => throw new NotImplementedException();
        /// <summary>
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRolesByUserId(int userId) => throw new NotImplementedException();
        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public virtual bool DeleteRole(IEnumerable<int> value) => throw new NotImplementedException();
        /// <summary>
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveRole(Role p) => throw new NotImplementedException();
        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRolesByMenuId(int menuId) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SavaRolesByMenuId(int id, IEnumerable<int> roleIds) => throw new NotImplementedException();
        /// <summary>
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrieveRolesByGroupId(int groupId) => throw new NotImplementedException();
        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveRolesByGroupId(int id, IEnumerable<int> roleIds) => throw new NotImplementedException();
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
