using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    [TableName("ResetUsers")]
    public class ResetUser
    {
        /// <summary>
        /// 获得/设置 用户主键ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string Reason { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public DateTime ResetTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Save(ResetUser user)
        {
            using var db = DbManager.Create();
            db.Save(user);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual ResetUser RetrieveUserByUserName(string userName)
        {
            using var db = DbManager.Create();
            return db.FirstOrDefault<ResetUser>("where UserName = @0 order by ResetTime desc", userName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<DateTime, string>> RetrieveResetReasonsByUserName(string userName)
        {
            using var db = DbManager.Create();
            return db.Fetch<ResetUser>("where UserName = @0 order by ResetTime desc", userName).Select(user => new KeyValuePair<DateTime, string>(user.ResetTime, user.Reason));
        }
    }
}
