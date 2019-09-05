using Bootstrap.Security;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.Client.DataAccess.MongoDB
{
    /// <summary>
    /// 字典表实体类
    /// </summary>
    internal class Dict : DataAccess.Dict
    {
        /// <summary>
        /// 获取所有字典表数据方法
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<BootstrapDict> RetrieveDicts() => DbManager.Dicts.Find(FilterDefinition<BootstrapDict>.Empty).ToList();
    }
}
