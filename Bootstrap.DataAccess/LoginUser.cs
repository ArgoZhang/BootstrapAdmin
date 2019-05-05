using Longbow.Web.Mvc;
using PetaPoco;
using System;

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
        /// <param name="ip"></param>
        /// <returns></returns>
        public virtual Page<LoginUser> Retrieves(PaginationOption po, string ip)
        {
            var sql = new Sql("select UserName, LoginTime, Ip, Browser, OS, City, Result from LoginLogs");
            if (!string.IsNullOrEmpty(ip)) sql.Where("ip = @0", ip);
            sql.OrderBy("LoginTime desc");
            return DbManager.Create().Page<LoginUser>(po.PageIndex, po.Limit, sql);
        }
    }
}
