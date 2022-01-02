using BootstrapAdmin.Web.Core;
using System.Collections.Specialized;
using System.Data.Common;

namespace BootstrapAdmin.Web.Utils;

/// <summary>
/// 
/// </summary>
public static class ExceptionsHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="ex"></param>
    /// <param name="additionalInfo"></param>
    /// <returns></returns>
    public static void Log(IServiceProvider provider, Exception? ex, NameValueCollection additionalInfo)
    {
        if (ex != null)
        {
            var errorPage = additionalInfo?["ErrorPage"] ?? (ex.GetType().Name.Length > 50 ? ex.GetType().Name.Substring(0, 50) : ex.GetType().Name);
            var loopEx = ex;
            var category = "App";
            while (loopEx != null)
            {
                if (typeof(DbException).IsAssignableFrom(loopEx.GetType()))
                {
                    category = "DB";
                    break;
                }
                loopEx = loopEx.InnerException;
            }
            var exception = new DataAccess.Models.Exception
            {
                AppDomainName = AppDomain.CurrentDomain.FriendlyName,
                ErrorPage = errorPage,
                UserId = additionalInfo?["UserId"],
                UserIp = additionalInfo?["UserIp"],
                ExceptionType = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                LogTime = DateTime.Now,
                Category = category
            };
            var expceptionService = provider.GetRequiredService<IException>();
            expceptionService.Log(exception);
        }
    }
}
