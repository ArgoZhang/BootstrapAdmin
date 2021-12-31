namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 登录服务
/// </summary>
public interface ILogin
{
    /// <summary>
    /// 记录登录日志
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    Task<bool> Log(string userName, bool result);
}
