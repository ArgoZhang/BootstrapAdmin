using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 应用程序实体类
    /// </summary>
    public class App : DataAccess.App
    {
        /// <summary>
        /// 获取所有应用程序数据方法
        /// </summary>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<string, string>> RetrieveApps() => DictHelper.RetrieveDicts().Where(d => d.Category == "应用程序" && d.Define == 0).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(d => d.Key);

        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUserName(string userName)
        {
            var ret = new List<string>();
            var user = UserHelper.RetrieveUserByUserName(userName) as User;
            if (user != null)
            {
                var roles = RoleHelper.Retrieves();

                // check administrators
                if (roles.Any(r => r.RoleName.Equals("Administrators", StringComparison.OrdinalIgnoreCase)))
                {
                    ret.AddRange(RetrieveApps().Select(kv => kv.Key));
                }
                else
                {
                    roles.Where(r => user.Roles.Any(role => role == r.Id)).ToList().ForEach(r => ret.AddRange(r.Apps));
                }
            }
            return ret;
        }
    }
}
