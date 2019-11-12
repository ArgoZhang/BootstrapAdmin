using Longbow.Cache;
using Longbow.Web.Mvc;
using PetaPoco;
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
        public static void Log(Exception ex, NameValueCollection additionalInfo)
        {
            var ret = DbContextManager.Create<Exceptions>()?.Log(ex, additionalInfo) ?? false;
            if (ret) CacheManager.Clear(RetrieveExceptionsDataKey);
        }

        /// <summary>
        /// 查询一周内所有异常
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Exceptions> Retrieves() => CacheManager.GetOrAdd(RetrieveExceptionsDataKey, key => DbContextManager.Create<Exceptions>()?.Retrieves()) ?? new Exceptions[0];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static Page<Exceptions> RetrievePages(PaginationOption po, DateTime? startTime, DateTime? endTime) => DbContextManager.Create<Exceptions>()?.RetrievePages(po, startTime, endTime) ?? new Page<Exceptions>() { Items = new List<Exceptions>() };
    }
}
