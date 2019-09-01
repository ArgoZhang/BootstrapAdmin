using Longbow.Cache;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 用户表相关操作帮助类
    /// </summary>
    internal class UserHelper
    {
        /// <summary>
        /// 获取所有用户缓存数据键值
        /// </summary>
        public const string RetrieveUsersDataKey = "UserHelper-RetrieveUsers";

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> Retrieves() => CacheManager.GetOrAdd(RetrieveUsersDataKey, key =>
        {
            var project = Builders<User>.Projection.Include(u => u.Id)
                .Include(u => u.UserName)
                .Include(u => u.DisplayName)
                .Include(u => u.Groups)
                .Include(u => u.Roles)
                .Include(u => u.ApprovedTime);
            return DbManager.Users.Find(user => user.ApprovedTime != DateTime.MinValue).Project<User>(project).ToList();
        });
    }
}
