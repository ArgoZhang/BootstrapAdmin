using Bootstrap.Security;
using Bootstrap.Security.DataAccess;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 用户表实体类
    /// </summary>
    public class User : BootstrapUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual BootstrapUser? RetrieveUserByUserName(string userName) => DbHelper.RetrieveUserByUserName(userName);
    }
}
