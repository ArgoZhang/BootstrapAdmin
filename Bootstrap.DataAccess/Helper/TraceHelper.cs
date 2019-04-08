using Longbow.Data;
using Longbow.Web;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Http;
using PetaPoco;
using System;

namespace Bootstrap.DataAccess
{
    public static class TraceHelper
    {

        /// <summary>
        /// 保存访问历史记录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="v"></param>
        public static void Save(HttpContext context, OnlineUser v)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var user = UserHelper.RetrieveUserByUserName(context.User.Identity.Name);
                v.UserName = user.UserName;
                v.DisplayName = user.DisplayName;
                DbContextManager.Create<Trace>().Save(new Trace
                {
                    Ip = v.Ip,
                    RequestUrl = v.RequestUrl,
                    LogTime = v.LastAccessTime,
                    City = v.Location,
                    Browser = v.Browser,
                    OS = v.OS,
                    UserName = v.UserName
                });
            }
        }

        /// <summary>
        /// 获得指定IP历史访问记录
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static Page<Trace> Retrieves(PaginationOption po, DateTime? startTime, DateTime? endTime) => DbContextManager.Create<Trace>().Retrieves(po, startTime, endTime);
    }
}
