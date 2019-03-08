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
        /// <returns></returns>
        public virtual IEnumerable<LoginUser> Retrieves() => DbManager.Create().Fetch<LoginUser>("Where LoginTime > @0 Order by LoginTime desc", DateTime.Today.AddMonths(0 - DictHelper.RetrieveLoginLogsPeriod()));
    }
}
