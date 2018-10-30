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
        public override IEnumerable<DataAccess.Log> RetrieveLogs()
        {
            var log = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.Log>("Logs");
            return log.Find(l => l.LogTime >= DateTime.Now.AddDays(-7)).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public override bool SaveLog(DataAccess.Log log)
        {
            var logs = MongoDbAccessManager.DBAccess.GetCollection<DataAccess.Log>("Logs");
            log.LogTime = DateTime.Now;
            logs.InsertOne(log);
            return true;
        }
    }
}
