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
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ResetTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Save()
        {
            var db = DbManager.Create();
            db.Save(this);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual ResetUser RetrieveUserByUserName(string userName) => DbManager.Create().FirstOrDefault<ResetUser>("where UserName = @0 order by ResetTime desc", userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual void DeleteByUserName(string userName) => DbManager.Create().Delete<ResetUser>("where UserName = @0", userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<DateTime, string>> RetrieveResetReasonsByUserName(string userName) => DbManager.Create().Fetch<ResetUser>("where UserName = @0 order by ResetTime desc", userName).Select(user => new KeyValuePair<DateTime, string>(user.ResetTime, user.Reason));
    }
}
