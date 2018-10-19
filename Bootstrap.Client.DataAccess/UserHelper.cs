using Bootstrap.Security;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 用户表相关操作类
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static BootstrapUser RetrieveUserByUserName(string userName)=> BAHelper.RetrieveUserByUserName(userName);
    }
}
