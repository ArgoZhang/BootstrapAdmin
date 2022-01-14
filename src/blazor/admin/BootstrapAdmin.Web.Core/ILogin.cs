namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 登录服务
/// </summary>
public interface ILogin
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="loginTime"></param>
    /// <param name="IP"></param>
    /// <param name="address"></param>
    /// <param name="browser"></param>
    /// <param name="userAgent"></param>
    /// <param name="OS"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    Task<bool> Log(string userName, DateTime loginTime, string IP, string address, string? browser, string userAgent, string OS, bool result);
}
