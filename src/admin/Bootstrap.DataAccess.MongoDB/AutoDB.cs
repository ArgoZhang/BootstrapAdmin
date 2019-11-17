using Bootstrap.Security;
using MongoDB.Driver;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 自动建库实体操作类
    /// </summary>
    public class AutoDB : DataAccess.AutoDB
    {
        /// <summary>
        /// 数据库检查方法
        /// <paramref name="folder"></paramref>
        /// </summary>
        public override void EnsureCreated(string folder)
        {
            // 检查数据库是否存在
            if (DbManager.Dicts.CountDocuments(FilterDefinition<BootstrapDict>.Empty) == 0) GenerateDB(folder);
        }
    }
}
