using Longbow.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Group
    {
        public const string RetrieveGroupsDataKey = "GroupHelper-RetrieveGroups";
        public const string RetrieveGroupsByUserIdDataKey = "GroupHelper-RetrieveGroupsByUserId";
        public const string RetrieveGroupsByRoleIdDataKey = "GroupHelper-RetrieveGroupsByRoleId";
        public const string RetrieveGroupsByUserNameDataKey = "BootstrapAdminGroupMiddleware-RetrieveGroupsByUserName";
        /// <summary>
        /// 获得/设置 群组主键ID
        /// </summary>
        public int Id { get; set; }

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
        public virtual IEnumerable<Group> RetrieveGroups(int id = 0) => throw new NotImplementedException();
        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="ids"></param>
        public virtual bool DeleteGroup(IEnumerable<int> value) => throw new NotImplementedException();
        /// <summary>
        /// 保存新建/更新的群组信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool SaveGroup(Group p) => throw new NotImplementedException();
        /// <summary>
        /// 根据用户查询部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Group> RetrieveGroupsByUserId(int userId) => throw new NotImplementedException();
        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public virtual bool SaveGroupsByUserId(int id, IEnumerable<int> groupIds) => throw new NotImplementedException();
        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Group> RetrieveGroupsByRoleId(int roleId) => throw new NotImplementedException();
        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public virtual bool SaveGroupsByRoleId(int id, IEnumerable<int> groupIds) => throw new NotImplementedException();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="connName"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveGroupsByUserName(string userName)
        {
            return CacheManager.GetOrAdd(string.Format("{0}-{1}", RetrieveGroupsByUserNameDataKey, userName), r =>
            {
                var entities = new List<string>();
                var db = DBAccessManager.DBAccess;
                using (DbCommand cmd = db.CreateCommand(CommandType.Text, "select g.GroupName, g.[Description] from Groups g inner join UserGroup ug on g.ID = ug.GroupID inner join Users u on ug.UserID = u.ID where UserName = @UserName"))
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
            }, RetrieveGroupsByUserNameDataKey);
        }
    }
}
