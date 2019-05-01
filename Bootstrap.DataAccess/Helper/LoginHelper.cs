using Longbow.Data;
using Longbow.Web.Mvc;
using PetaPoco;

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

        /// <summary>
        /// 查询所有登录日志
        /// </summary>
        /// <param name="po"></param>
        public static Page<LoginUser> Retrieves(PaginationOption po) => DbContextManager.Create<LoginUser>().Retrieves(po);
    }
}
