using Bootstrap.Security.Mvc;
using Longbow.Data;
using Microsoft.Extensions.Configuration;
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
        public static IDatabase Create(string? connectionName = null, bool keepAlive = false)
        {
            var db = Longbow.Data.DbManager.Create(connectionName, keepAlive);
            db.ExceptionThrown += (sender, args) => args.Exception.Log(new NameValueCollection() { ["LastCmd"] = db.LastCommand });
            return db;
        }

        /// <summary>
        /// 创建 Sqlite 类型的 IDatabase 实例
        /// </summary>
        /// <param name="connectionName">配置文件中配置的数据库连接字符串名称</param>
        /// <param name="keepAlive">是否保持连接，默认为 false</param>
        /// <returns></returns>
        public static IDatabase CreateSqlite(string? connectionName = "client", bool keepAlive = false)
        {
            // 此方法为演示同时连接不同的数据库操作

            // 此处注释为获取连接字符串的不同方法
            //var conn = Bootstrap.Security.Mvc.BootstrapAppContext.Configuration["ConnectionStrings:client"];
            //var conn = Bootstrap.Security.Mvc.BootstrapAppContext.Configuration.GetSection("ConnectionStrings").GetValue("client", "");

            var conn = BootstrapAppContext.Configuration.GetConnectionString(connectionName);
            var db = Longbow.Data.DbManager.Create(new DatabaseOption()
            {
                ProviderName = DatabaseProviderType.SQLite,
                ConnectionString = conn,
                KeepAlive = keepAlive
            });
            db.ExceptionThrown += (sender, args) => args.Exception.Log(new NameValueCollection() { ["LastCmd"] = db.LastCommand });
            return db;
        }
    }
}
