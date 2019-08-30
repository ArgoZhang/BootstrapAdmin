using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    class Group : BootstrapGroup
    {
        /// <summary>
        /// 获得/设置 群组描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Group> Retrieves()
        {
            return DbManager.Groups.Find(FilterDefinition<Group>.Empty).ToList();
        }
    }
}
