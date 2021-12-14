using Bootstrap.Security.Blazor;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Services
{
    class AdminService : IBootstrapAdminService
    {
        private IUsers User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public AdminService(IUsers user) => User = user;

        /// <summary>
        /// 通过当前登录用户名获取角色集合方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetRoles(string userName) => User.GetRoles(userName);

        /// <summary>
        /// 通过当前登录用户名获取授权 App 集合方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetApps(string userName) => User.GetApps(userName);
    }
}
