using Bootstrap.Security.DataAccess;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class App
    {
        /// <summary>
        /// 根据指定用户名获得授权应用程序集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> RetrievesByUserName(string userName) => DbHelper.RetrieveAppsByUserName(userName);
    }
}
