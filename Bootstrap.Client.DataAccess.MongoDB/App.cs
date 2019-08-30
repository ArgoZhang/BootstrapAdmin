using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class App : DataAccess.App
    {
        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<string> RetrievesByUserName(string userName)
        {
            var ret = new List<string>();
            var roles = RoleHelper.RetrievesByUserName(userName);
            if (roles.Contains("Administrators", StringComparer.OrdinalIgnoreCase))
            {
                ret.AddRange(DictHelper.RetrieveApps().Select(kv => kv.Key));
            }
            else
            {
                RoleHelper.Retrieves().Cast<Role>().Where(r => roles.Any(rn => rn == r.RoleName)).ToList().ForEach(r => ret.AddRange(r.Apps));
            }
            return ret;
        }
    }
}
