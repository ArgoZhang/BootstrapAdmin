namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface ILogin
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    Task<bool> Log(string userName, bool result);
}
