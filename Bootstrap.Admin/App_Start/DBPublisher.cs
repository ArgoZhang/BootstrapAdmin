using Bootstrap.DataAccess;
using Longbow.Caching;
using Longbow.ExceptionManagement;
using Longbow.ExceptionManagement.Configuration;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Bootstrap.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class DBPublisher : IExceptionPublisher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <param name="publisherElement"></param>
        public void Publish(Exception ex, NameValueCollection additionalInfo, ExceptionPublisherElement publisherElement)
        {
            if (publisherElement.Mode == PublisherMode.Off) return;
            HttpContext context = HttpContext.Current;
            additionalInfo["ErrorPage"] = context.Request.AppRelativeCurrentExecutionFilePath;
            if (context.User != null) additionalInfo["UserId"] = context.User.Identity.Name;
            additionalInfo["UserIp"] = context.Request.UserHostAddress;
            ExceptionHelper.Log(ex, additionalInfo);
            CacheManager.Clear();
        }
    }
}