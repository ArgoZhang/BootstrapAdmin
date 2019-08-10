using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 菜单实体类
    /// </summary>
    [TableName("Navigations")]
    public class Menu : BootstrapMenu
    {
        /// <summary>
        /// 删除菜单信息
        /// </summary>
        /// <param name="value"></param>
        public virtual bool Delete(IEnumerable<string> value)
        {
            if (!value.Any()) return true;
            var ret = false;
            var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                db.Execute($"delete from NavigationRole where NavigationID in @value", new { value });
                db.Delete<Menu>($"where ID in @value", new { value });
                db.CompleteTransaction();
                ret = true;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// 保存新建/更新的菜单信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Save(BootstrapMenu p)
        {
            if (string.IsNullOrEmpty(p.Name)) throw new ArgumentNullException(nameof(p.Name));

            if (p.Name.Length > 50) p.Name = p.Name.Substring(0, 50);
            if (p.Icon != null && p.Icon.Length > 50) p.Icon = p.Icon.Substring(0, 50);
            if (p.Url != null && p.Url.Length > 4000) p.Url = p.Url.Substring(0, 4000);
            DbManager.Create().Save(p);
            return true;
        }

        /// <summary>
        /// 查询某个角色所配置的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveMenusByRoleId(string roleId)
        {
            var menus = DbManager.Create().Fetch<BootstrapMenu>("select NavigationID as Id from NavigationRole where RoleID = @0", roleId);
            return menus.Select(m => m.Id);
        }

        /// <summary>
        /// 通过角色ID保存当前授权菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public virtual bool SaveMenusByRoleId(string roleId, IEnumerable<string> menuIds)
        {
            bool ret = false;
            var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                db.Execute("delete from NavigationRole where RoleID = @0", roleId);
                db.InsertBatch("NavigationRole", menuIds.Select(g => new { NavigationID = g, RoleID = roleId }));
                db.CompleteTransaction();
                ret = true;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// 通过当前用户名获得所有菜单
        /// </summary>
        /// <param name="userName">当前登陆的用户名</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName) => DbHelper.RetrieveAllMenus(userName);
    }
}
