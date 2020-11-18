using Bootstrap.Security.DataAccess;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 角色实体类
    /// </summary>
    [TableName("Roles")]
    public class Role
    {
        /// <summary>
        /// 获得/设置 角色主键ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 角色名称
        /// </summary>
        [DisplayName("角色名称")]
        public string RoleName { get; set; } = "";

        /// <summary>
        /// 获得/设置 角色描述
        /// </summary>
        [DisplayName("角色描述")]
        public string Description { get; set; } = "";

        /// <summary>
        /// 获取/设置 用户角色关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        [ResultColumn]
        public string Checked { get; set; } = "";

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Role> Retrieves()
        {
            using var db = DbManager.Create();
            return db.Fetch<Role>();
        }

        /// <summary>
        /// 保存用户角色关系
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveByUserId(string userId, IEnumerable<string> roleIds)
        {
            var ret = false;
            using var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                // delete user from config table
                db.Execute("delete from UserRole where UserID = @0", userId);
                db.InsertBatch("UserRole", roleIds.Select(g => new { UserID = userId, RoleID = g }));
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
        /// 查询某个用户所拥有的角色
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrievesByUserId(string userId)
        {
            using var db = DbManager.Create();
            return db.Fetch<Role>("select r.ID, r.RoleName, r.Description, case ur.RoleID when r.ID then 'checked' else '' end Checked from Roles r left join UserRole ur on r.ID = ur.RoleID and UserID = @0", userId);
        }

        /// <summary>
        /// 删除角色表
        /// </summary>
        /// <param name="value"></param>
        public virtual bool Delete(IEnumerable<string> value)
        {
            if (!value.Any()) return true;
            var ret = false;
            var ids = string.Join(",", value);
            using var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                db.Execute($"delete from UserRole where RoleID in ({ids})");
                db.Execute($"delete from RoleGroup where RoleID in ({ids})");
                db.Execute($"delete from NavigationRole where RoleID in ({ids})");
                db.Delete<Role>($"where ID in ({ids})");
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
        /// 保存新建/更新的角色信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Save(Role p)
        {
            if (!string.IsNullOrEmpty(p.RoleName) && p.RoleName.Length > 50) p.RoleName = p.RoleName.Substring(0, 50);
            if (!string.IsNullOrEmpty(p.Description) && p.Description.Length > 50) p.Description = p.Description.Substring(0, 500);

            using var db = DbManager.Create();
            db.Save(p);
            return true;
        }

        /// <summary>
        /// 查询某个菜单所拥有的角色
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrievesByMenuId(string menuId)
        {
            using var db = DbManager.Create();
            return db.Fetch<Role>("select r.ID, r.RoleName, r.Description, case ur.RoleID when r.ID then 'checked' else '' end Checked from Roles r left join NavigationRole ur on r.ID = ur.RoleID and NavigationID = @0", menuId);
        }

        /// <summary>
        /// 通过指定菜单 ID 保存角色集合数据
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SavaByMenuId(string menuId, IEnumerable<string> roleIds)
        {
            // 参数 id 可能是子菜单
            // https://gitee.com/LongbowEnterprise/dashboard/issues?id=IQW93

            var ret = false;
            using var db = DbManager.Create();
            db.BeginTransaction();
            try
            {
                string? parentId = menuId;
                if (!string.IsNullOrEmpty(parentId))
                {
                    do
                    {
                        // delete role from config table
                        db.Execute("delete from NavigationRole where NavigationID = @0", parentId);
                        db.InsertBatch("NavigationRole", roleIds.Select(g => new { NavigationID = parentId, RoleID = g }));

                        // find parent Menu Id
                        parentId = db.ExecuteScalar<string?>("select ParentId from Navigations Where Id = @0", parentId);
                    }
                    while (!string.IsNullOrEmpty(parentId) && parentId != "0");
                }
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
        /// 根据GroupId查询和该Group有关的所有Roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Role> RetrievesByGroupId(string groupId)
        {
            using var db = DbManager.Create();
            return db.Fetch<Role>("select r.ID, r.RoleName, r.Description, case ur.RoleID when r.ID then 'checked' else '' end Checked from Roles r left join RoleGroup ur on r.ID = ur.RoleID and GroupID = @0", groupId);
        }

        /// <summary>
        /// 根据GroupId更新Roles信息，删除旧的Roles信息，插入新的Roles信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual bool SaveByGroupId(string groupId, IEnumerable<string> roleIds)
        {
            var ret = false;
            var db = DbManager.Create();
            try
            {
                // delete user from config table
                db.Execute("delete from RoleGroup where GroupID = @0", groupId);
                db.InsertBatch("RoleGroup", roleIds.Select(g => new { GroupID = groupId, RoleID = g }));
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
        /// 通过指定登录用户名获得角色列表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrievesByUserName(string userName) => DbHelper.RetrieveRolesByUserName(userName);

        /// <summary>
        /// 根据菜单url查询某个所拥有的角色
        /// 从NavigatorRole表查
        /// 从Navigators -> GroupNavigatorRole -> Role查查询某个用户所拥有的角色
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrievesByUrl(string url, string appId) => DbHelper.RetrieveRolesByUrl(url, appId);
    }
}
