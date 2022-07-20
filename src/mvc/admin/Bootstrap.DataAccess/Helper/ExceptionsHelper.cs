// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.Cache;
using Longbow.Web.Mvc;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

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
        /// <param name="provider"></param>
        /// <param name="eventId"></param>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public static void Log(IServiceProvider provider, EventId eventId, Exception? ex, NameValueCollection additionalInfo)
        {
            if (ex != null)
            {
                var ret = DbContextManager.Create<Exceptions>()?.Log(ex, additionalInfo) ?? false;
                if (ret)
                {
                    CacheManager.Clear(RetrieveExceptionsDataKey);
                }
            }
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

        /// <summary>
        /// 获得 Error 错误日志目录下所有文件
        /// </summary>
        public static IEnumerable<string> RetrieveLogFiles()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Error");
            return Directory.Exists(filePath)
                ? Directory.GetFiles(filePath)
                .Where(f => Path.GetExtension(f).Equals(".log", StringComparison.OrdinalIgnoreCase))
                .Select(f => Path.GetFileNameWithoutExtension(f)).OrderByDescending(s => s)
                : Enumerable.Empty<string>();
        }
    }
}
