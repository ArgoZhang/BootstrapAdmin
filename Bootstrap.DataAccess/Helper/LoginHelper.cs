using Longbow.Data;
using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoginHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Log(LoginUser user)
        {
            if (user.Id == string.Empty) user.Id = null;
            if (string.IsNullOrEmpty(user.UserName)) user.UserName = user.Ip;
            return DbContextManager.Create<LoginUser>().Log(user);
        }

        /// <summary>
        /// 查询指定页码登录日志
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        public static Page<LoginUser> RetrievePages(PaginationOption po, DateTime? startTime, DateTime? endTime, string ip) => DbContextManager.Create<LoginUser>().RetrieveByPages(po, startTime, endTime, ip);

        /// <summary>
        /// 查询所有登录日志
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IEnumerable<LoginUser> RetrieveAll(DateTime? startTime, DateTime? endTime, string ip)
        {
            return DbContextManager.Create<LoginUser>().RetrieveAll(startTime, endTime, ip);
        }
    }
}
