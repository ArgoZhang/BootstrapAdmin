using Longbow.Data;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoginHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Log(LoginUser user) => DbContextManager.Create<LoginUser>().Log(user);
    }
}
