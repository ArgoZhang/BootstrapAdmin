using Longbow.Data;
using System;
using System.Collections.Generic;

namespace Bootstrap.Client.DataAccess
{
    /// <summary>
    /// 示例操作帮助类
    /// </summary>
    public static class DummyHelper
    {
        /// <summary>
        /// 获取数据库中所有 Dummy 表数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Dummy> Retrieves()
        {
            // 此处启用智能切换数据库功能
            return DbContextManager.Create<Dummy>()?.Retrieves() ?? Array.Empty<Dummy>();
        }

        /// <summary>
        /// 保存 Dummy 实例到数据库中
        /// </summary>
        /// <param name="dummy"></param>
        /// <returns></returns>
        public static bool Save(Dummy dummy)
        {
            // 此处未启用智能
            using var db = DbManager.Create("client");
            db.Save(dummy);
            return true;
        }

        /// <summary>
        /// 删除指定 ID 的 Dummy 数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool Delete(IEnumerable<string> ids)
        {
            // 此处使用指定 Sqlite 数据方法执行数据库操作 演示同一个程序操作多个数据库的场景
            using var db = DbManager.CreateSqlite("client");
            db.Delete<Dummy>("where Id in (@ids)", new { ids });
            return true;
        }
    }
}
