using Bootstrap.Security;
using Bootstrap.Security.DataAccess;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 菜单实体类
    /// </summary>
    public class Menu : BootstrapMenu
    {
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
