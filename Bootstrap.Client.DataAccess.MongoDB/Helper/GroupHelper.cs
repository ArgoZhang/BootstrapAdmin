using Longbow.Cache;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 组数据相关操作帮助类
    /// </summary>
    internal class GroupHelper
    {
        /// <summary>
        /// 组数据缓存键值
        /// </summary>
        public const string RetrieveGroupsDataKey = "GroupHelper-RetrieveGroups";

        /// <summary>
        /// 获取所有组数据集合方法
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Group> Retrieves()
        {
            return CacheManager.GetOrAdd(RetrieveGroupsDataKey, key => DbManager.Groups.Find(FilterDefinition<Group>.Empty).ToList());
        }
    }
}
