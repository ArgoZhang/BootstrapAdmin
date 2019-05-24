using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    [TableName("LoginLogs")]
    public class LoginUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool Log(LoginUser user)
        {
            var db = DbManager.Create();
            db.Save(user);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public virtual Page<LoginUser> RetrieveByPages(PaginationOption po, DateTime? startTime, DateTime? endTime, string ip)
        {
            var sql = new Sql("select UserName, LoginTime, Ip, Browser, OS, City, Result from LoginLogs");
            if (startTime.HasValue) sql.Where("LoginTime >= @0", startTime.Value);
            if (endTime.HasValue) sql.Where("LoginTime < @0", endTime.Value.AddDays(1));
            if (!string.IsNullOrEmpty(ip)) sql.Where("ip = @0", ip);
            sql.OrderBy($"{po.Sort} {po.Order}");
            return DbManager.Create().Page<LoginUser>(po.PageIndex, po.Limit, sql);
        }

        /// <summary>
        /// 获取所有登录数据
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<LoginUser> Retrieves(DateTime? startTime, DateTime? endTime, string ip) => DbManager.Create().Fetch<LoginUser>();
    }
}
