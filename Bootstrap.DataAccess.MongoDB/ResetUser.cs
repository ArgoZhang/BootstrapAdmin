using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetUser : DataAccess.ResetUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override DataAccess.ResetUser RetrieveUserByUserName(string userName) => DbManager.ResetUsers.Find(user => user.UserName.ToLowerInvariant() == userName.ToLowerInvariant()).FirstOrDefault();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override IEnumerable<KeyValuePair<DateTime, string>> RetrieveResetReasonsByUserName(string userName) => DbManager.ResetUsers.Find(user => user.UserName.ToLowerInvariant() == userName.ToLowerInvariant()).ToList().OrderByDescending(user => user.ResetTime).Select(user => new KeyValuePair<DateTime, string>(user.ResetTime, user.Reason));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Save()
        {
            DbManager.ResetUsers.InsertOne(this);
            return true;
        }
    }
}
