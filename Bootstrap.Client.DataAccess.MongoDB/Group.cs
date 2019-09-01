using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// Group 实体类
    /// </summary>
    internal class Group : BootstrapGroup
    {
        /// <summary>
        /// 获得/设置 群组描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获得/设置 当前组授权角色数据集合
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// 获得所有组数据方法
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Group> Retrieves()
        {
            return DbManager.Groups.Find(FilterDefinition<Group>.Empty).ToList();
        }
    }
}
