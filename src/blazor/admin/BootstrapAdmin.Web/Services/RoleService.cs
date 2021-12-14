using Bootstrap.Security.Blazor;

namespace BootstrapAdmin.Web.Services
{
    class RoleService : IRoleService
    {
        /// <summary>
        /// 通过当前登录用户名获取角色集合方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetRolesByUserName(string userName) => new List<string>() { "Administrators" };
    }
}
