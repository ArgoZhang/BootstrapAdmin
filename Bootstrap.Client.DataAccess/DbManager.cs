using PetaPoco;
using System;
using System.Collections.Specialized;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 数据库连接操作类
    /// </summary>
    internal static class DbManager
    {
        /// <summary>
        /// 创建 IDatabase 实例方法
        /// </summary>
        /// <param name="connectionName">配置文件中配置的数据库连接字符串名称</param>
        /// <param name="keepAlive">是否保持连接，默认为 false</param>
        /// <returns></returns>
        public static IDatabase Create(string connectionName = null, bool keepAlive = false)
        {
            var db = Longbow.Data.DbManager.Create(connectionName, keepAlive);
            db.ExceptionThrown += (sender, args) => args.Exception.Log(new NameValueCollection() { ["LastCmd"] = db.LastCommand });
            return db;
        }
    }
}
