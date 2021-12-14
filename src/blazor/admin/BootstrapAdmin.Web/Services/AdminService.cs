using Bootstrap.Security.Blazor;

namespace BootstrapAdmin.Web.Services
{
    class AdminService : IBootstrapAdminService
    {
        /// <summary>
        /// 通过当前登录用户名获取角色集合方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetRolesByUserName(string userName) => new List<string>() { "Administrators" };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetAppsByUserName(string userName) => new List<string>() { "BA" };
    }
}
