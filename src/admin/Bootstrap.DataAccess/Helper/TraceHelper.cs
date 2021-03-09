using Longbow.Web;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Http;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户跟踪操作类
    /// </summary>
    public static class TraceHelper
    {
        /// <summary>
        /// 保存访问历史记录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="v"></param>
        public static void Save(HttpContext context, OnlineUser v)
        {
            if (context.User.Identity!.IsAuthenticated)
            {
                var user = UserHelper.RetrieveUserByUserName(context.User.Identity.Name);

                // user == null 以前登录过客户端保留了 Cookie 但是用户名可能被系统删除
                // link bug: https://gitee.com/dotnetchina/BootstrapAdmin/issues/I123MH
                if (user != null)
                {
                    v.UserName = user.UserName;
                    v.DisplayName = user.DisplayName;
                    DbContextManager.Create<Trace>()?.Save(new Trace
                    {
                        Ip = v.Ip,
                        RequestUrl = v.RequestUrl,
                        LogTime = v.LastAccessTime,
                        City = v.Location,
                        Browser = v.Browser,
                        OS = v.OS,
                        UserName = v.UserName,
                        UserAgent = v.UserAgent,
                        Referer = v.Referer
                    });
                }
            }
        }

        /// <summary>
        /// 进入在线跟踪的地址过滤方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool Filter(HttpContext context)
        {
            var url = context.Request.Path;
            return !new string[] { "/api", "/lib", "/NotiHub", "/Healths", "/healths-ui", "/TaskLogHub", "/swagger", "/CacheList.axd", "/_blazor" }.Any(r => url.StartsWithSegments(r, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获得指定IP历史访问记录
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static Page<Trace> Retrieves(PaginationOption po, DateTime? startTime, DateTime? endTime, string? ip) => DbContextManager.Create<Trace>()?.RetrievePages(po, startTime, endTime, ip) ?? new Page<Trace>() { Items = new List<Trace>() };

        /// <summary>
        /// 获得指定IP历史访问记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IEnumerable<Trace> RetrieveAll(DateTime? startTime, DateTime? endTime, string? ip) => DbContextManager.Create<Trace>()?.RetrieveAll(startTime, endTime, ip) ?? new Trace[0];
    }
}
