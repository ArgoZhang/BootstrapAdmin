using Bootstrap.Security.DataAccess;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 
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
