using Bootstrap.Security.Blazor;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminService : IBootstrapAdminService
    {
        private IUsers User { get; set; }

        private INavigations Navigations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="navigations"></param>
        public AdminService(IUsers user, INavigations navigations)
        {
            User = user;
            Navigations = navigations;
        }

        /// <summary>
        /// 通过用户名获取角色集合方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetRoles(string userName) => User.GetRoles(userName);

        /// <summary>
        /// 通过用户名获取授权 App 集合方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetApps(string userName) => User.GetApps(userName);

        /// <summary>
        /// 通过用户名检查当前请求 Url 是否已授权方法
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<bool> AuhorizingNavigation(string userName, string url)
        {
            var ret = false;
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            {
                ret = Navigations.GetAllMenus(userName)
                    .Any(m => m.Url.Contains(uri.AbsolutePath, StringComparison.OrdinalIgnoreCase));
            }
            return Task.FromResult(ret);
        }
    }
}
