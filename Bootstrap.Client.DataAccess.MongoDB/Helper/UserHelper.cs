using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    class UserHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> Retrieves()
        {
            var project = Builders<User>.Projection.Include(u => u.Id)
                .Include(u => u.UserName)
                .Include(u => u.DisplayName)
                .Include(u => u.Groups)
                .Include(u => u.Roles)
                .Include(u => u.ApprovedTime);
            return DbManager.Users.Find(user => user.ApprovedTime != DateTime.MinValue).Project<User>(project).ToList();
        }
    }
}
