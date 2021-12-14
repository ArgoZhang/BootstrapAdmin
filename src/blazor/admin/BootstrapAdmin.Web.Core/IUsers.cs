namespace BootstrapAdmin.Web.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsers
    {
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
    }
}
