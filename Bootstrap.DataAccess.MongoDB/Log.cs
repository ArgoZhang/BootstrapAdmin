using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Log : DataAccess.Log
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Log> Retrieves() => MongoDbAccessManager.Logs.Find(l => l.LogTime >= DateTime.Now.AddDays(-7)).ToList();
        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static void DeleteLogAsync() => System.Threading.Tasks.Task.Run(() => MongoDbAccessManager.Logs.DeleteMany(log => log.LogTime < DateTime.Now.AddDays(-7)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.Log log)
        {
            log.LogTime = DateTime.Now;
            MongoDbAccessManager.Logs.InsertOne(log);
            return true;
        }
    }
}
