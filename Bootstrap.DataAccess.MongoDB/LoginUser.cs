using MongoDB.Driver;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginUser : DataAccess.LoginUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool Log(DataAccess.LoginUser user)
        {
            DbManager.LoginUsers.InsertOne(user);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.LoginUser> Retrieves() => DbManager.LoginUsers.Find(FilterDefinition<DataAccess.LoginUser>.Empty).SortByDescending(user => user.LoginTime).ToList();
    }
}
