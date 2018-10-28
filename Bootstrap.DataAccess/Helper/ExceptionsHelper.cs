using Longbow.Cache;
using Longbow.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string RetrieveExceptionsDataKey = "ExceptionHelper-RetrieveExceptions";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public static void Log(Exception ex, NameValueCollection additionalInfo) => DbAdapterManager.Create<Exceptions>().Log(ex, additionalInfo);
        /// <summary>
        /// 查询一周内所有异常
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Exceptions> RetrieveExceptions() => CacheManager.GetOrAdd(RetrieveExceptionsDataKey, key => DbAdapterManager.Create<Exceptions>().RetrieveExceptions());
    }
}
