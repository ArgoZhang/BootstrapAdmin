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
            using var db = DbManager.Create();
            bool ret;
            try
            {
                db.BeginTransaction();
                db.Execute($"delete from NavigationRole where NavigationID in (@value)", new { value });
                db.Delete<Menu>($"where ID in (@value)", new { value });
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
            using var db = DbManager.Create();
            db.Save(p);
            return true;
        }

        /// <summary>
        /// 查询某个角色所配置的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrieveMenusByRoleId(string roleId)
        {
            using var db = DbManager.Create();
            var menus = db.Fetch<BootstrapMenu>("select NavigationID as Id from NavigationRole where RoleID = @0", roleId);
#pragma warning disable CS8619 // 值中的引用类型的为 Null 性与目标类型不匹配。
            return menus.Select(m => m.Id);
#pragma warning restore CS8619 // 值中的引用类型的为 Null 性与目标类型不匹配。
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
            using var db = DbManager.Create();
            try
            {
                db.BeginTransaction();
                db.Execute("delete from NavigationRole where RoleID = @0", roleId);
                db.InsertBatch("NavigationRole", menuIds.Select(g => new { NavigationID = g, RoleID = roleId }));
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
        /// 通过当前用户名获得所有菜单
        /// </summary>
        /// <param name="userName">当前登录的用户名</param>
        /// <returns></returns>
        public virtual IEnumerable<BootstrapMenu> RetrieveAllMenus(string userName) => DbHelper.RetrieveAllMenus(userName);

        /// <summary>
        /// 通过当前用户名与指定菜单路径获取此菜单下所有授权按钮集合 (userName, url, auths) => bool
        /// </summary>
        /// <param name="userName">当前操作用户名</param>
        /// <param name="url">资源按钮所属菜单</param>
        /// <param name="auths">资源授权码</param>
        /// <returns></returns>
        public virtual bool AuthorizateButtons(string userName, string url, string auths)
        {
            var menus = MenuHelper.RetrieveAllMenus(userName);
            return DbHelper.AuthorizateButtons(menus, url, auths);
        }
    }
}
