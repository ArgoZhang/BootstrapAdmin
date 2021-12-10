using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    [TableName("Groups")]
    public class Group : BootstrapGroup
    {
        /// <summary>
        /// 获得/设置 群组描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 获取/设置 用户群组关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        [ResultColumn]
        public string Checked { get; set; } = "";

        /// <summary>
        /// 查询所有群组信息
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Group> Retrieves()
        {
            using var db = DbManager.Create();
            return db.Fetch<Group>();
        }

        /// <summary>
        /// 删除群组信息
        /// </summary>
        /// <param name="value"></param>
        public virtual bool Delete(IEnumerable<string> value)
        {
            var ids = string.Join(",", value);
            using var db = DbManager.Create();
            bool ret;
            try
            {
                db.BeginTransaction();
                db.Execute($"delete from UserGroup where GroupID in ({ids})");
                db.Execute($"delete from RoleGroup where GroupID in ({ids})");
                db.Delete<Group>($"where ID in ({ids})");
                db.CompleteTransaction();
                ret = true;
            }
            catch (Exception)
            {
                db.AbortTransaction();
                throw;
            }
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的群组信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Save(Group p)
        {
            using var db = DbManager.Create();
            db.Save(p);
            return true;
        }

        /// <summary>
        /// 根据用户查询部门信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Group> RetrievesByUserId(string userId)
        {
            using var db = DbManager.Create();
            return db.Fetch<Group>($"select g.ID, g.GroupCode, g.GroupName, g.Description, case ug.GroupID when g.ID then 'checked' else '' end Checked from {db.Provider.EscapeSqlIdentifier("Groups")} g left join UserGroup ug on g.ID = ug.GroupID and UserID = @0", userId);
        }

        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Group> RetrievesByRoleId(string roleId)
        {
            using var db = DbManager.Create();
            return db.Fetch<Group>($"select g.ID, g.GroupCode, g.GroupName, g.Description, case rg.GroupID when g.ID then 'checked' else '' end Checked from {db.Provider.EscapeSqlIdentifier("Groups")} g left join RoleGroup rg on g.ID = rg.GroupID and RoleID = @0", roleId);
        }

        /// <summary>
        /// 保存用户部门关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public virtual bool SaveByUserId(string userId, IEnumerable<string> groupIds)
        {
            var ret = false;
            var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                //删除用户部门表中该用户所有的部门关系
                db.Execute("delete from UserGroup where UserID = @0", userId);
                db.InsertBatch("UserGroup", groupIds.Select(g => new { UserID = userId, GroupID = g }));
                db.CompleteTransaction();
                ret = true;
            }
            catch (Exception)
            {
                db.AbortTransaction();
                throw;
            }
            return ret;
        }

        /// <summary>
        /// 根据角色ID以及选定的部门ID，保到角色部门表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public virtual bool SaveByRoleId(string roleId, IEnumerable<string> groupIds)
        {
            bool ret = false;
            var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                //删除角色部门表该角色所有的部门
                db.Execute("delete from RoleGroup where RoleID = @0", roleId);
                db.InsertBatch("RoleGroup", groupIds.Select(g => new { RoleID = roleId, GroupID = g }));
                db.CompleteTransaction();
                ret = true;
            }
            catch (Exception)
            {
                db.AbortTransaction();
                throw;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapGroup> RetrievesByUserName(string userName) => DbHelper.RetrieveGroupsByUserName(userName);
    }
}
