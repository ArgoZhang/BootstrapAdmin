using BootstrapAdmin.DataAccess.Models;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.Web.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        string? GetDisplayName(string? userName);

        /// <summary>
        /// 通过用户名获取角色列表
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<string> GetRoles(string userName);

        /// <summary>
        /// 通过用户名获得授权 App 集合
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<string> GetApps(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<string> GetUsersByGroupId(string? id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SaveUsersByGroupId(string? id, IEnumerable<string> userIds);

        /// <summary>
        /// 获得所有用户
        /// </summary>
        /// <returns></returns>
        List<User> GetAll();

        /// <summary>
        /// 认证方法
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Authenticate(string userName, string password);
    }
}
