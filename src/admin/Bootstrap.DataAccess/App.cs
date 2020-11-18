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
    public class App
    {
        /// <summary>
        /// 获得/设置 应用程序主键ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 群组名称
        /// </summary>
        public string AppName { get; set; } = "未设置";

        /// <summary>
        /// 获取/设置 用户群组关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        public string Checked { get; set; } = "";

        /// <summary>
        /// 根据角色ID指派部门
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<App> RetrievesByRoleId(string roleId)
        {
            using var db = DbManager.Create();
            var ret = db.Fetch<App>($"select d.Code as Id, d.Name as AppName, case ra.AppId when d.Code then 'checked' else '' end Checked from Dicts d left join RoleApp ra on d.Code = ra.AppId and ra.RoleId = @1 where d.Category = @0", "应用程序", roleId);

            // 判断是否为Administrators
            var role = RoleHelper.Retrieves().FirstOrDefault(r => r.Id == roleId);
            if (role != null && role.RoleName.Equals("Administrators", StringComparison.OrdinalIgnoreCase))
            {
                ret.ForEach(r => r.Checked = "checked");
            }
            return ret;
        }

        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrievesByUserName(string userName) => DbHelper.RetrieveAppsByUserName(userName);

        /// <summary>
        /// 根据角色ID以及选定的App ID，保到角色应用表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="appIds"></param>
        /// <returns></returns>
        public virtual bool SaveByRoleId(string roleId, IEnumerable<string> appIds)
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(nameof(roleId));

            bool ret = false;
            if (appIds == null) appIds = new string[0];
            var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                //删除角色应用表该角色所有的应用
                db.Execute("delete from RoleApp where RoleID = @0", roleId);
                db.InsertBatch("RoleApp", appIds.Select(g => new { RoleID = roleId, AppID = g }));
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
    }
}
