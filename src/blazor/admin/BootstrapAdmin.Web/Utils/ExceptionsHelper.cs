// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
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
    /// <param name="eventId"></param>
    /// <param name="ex"></param>
    /// <param name="additionalInfo"></param>
    /// <returns></returns>
    public static void Log(IServiceProvider provider, EventId eventId, Exception? ex, NameValueCollection additionalInfo)
    {
        // 1001 为 DB 异常防止循环调用
        if (ex != null && eventId.Id != 1001)
        {
            // 数据库长度 50 需要截取
            var errorPage = ex.GetType().Name.Length > 50
                ? ex.GetType().Name[..50]
                : ex.GetType().Name;

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
            var exception = new Error
            {
                AppDomainName = AppDomain.CurrentDomain.FriendlyName,
                ErrorPage = errorPage,
                UserId = "",
                UserIp = "",
                ExceptionType = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                LogTime = DateTime.Now,
                Category = category
            };
            //var expceptionService = provider.GetRequiredService<IException>();
            //expceptionService.Log(exception);
        }
    }
}
