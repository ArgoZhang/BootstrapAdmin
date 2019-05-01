using Longbow.Web.Mvc;
using MongoDB.Driver;
using PetaPoco;
using System;
using System.Linq;

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
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="opType"></param>
        /// <returns></returns>
        public override Page<DataAccess.Log> Retrieves(PaginationOption po, DateTime? startTime, DateTime? endTime, string opType)
        {
            var filterBuilder = Builders<DataAccess.Log>.Filter;
            var filter = filterBuilder.Empty;
            if (startTime.HasValue) filter = filterBuilder.Gte("LogTime", startTime.Value);
            if (endTime.HasValue) filter = filterBuilder.Lt("LogTime", endTime.Value.AddDays(1).AddSeconds(-1));
            if (startTime == null && endTime == null) filter = filterBuilder.Gt("LogTime", DateTime.Today.AddMonths(0 - DictHelper.RetrieveAccessLogPeriod()));
            if (!string.IsNullOrEmpty(opType)) filter = filterBuilder.Eq("CRUD", opType);

            // sort
            var sortBuilder = Builders<DataAccess.Log>.Sort;
            SortDefinition<DataAccess.Log> sort = null;
            switch (po.Sort)
            {
                case "CRUD":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.CRUD) : sortBuilder.Descending(t => t.CRUD);
                    break;
                case "UserName":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.UserName) : sortBuilder.Descending(t => t.UserName);
                    break;
                case "LogTime":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.LogTime) : sortBuilder.Descending(t => t.LogTime);
                    break;
                case "Ip":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.Ip) : sortBuilder.Descending(t => t.Ip);
                    break;
                case "RequestUrl":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.RequestUrl) : sortBuilder.Descending(t => t.RequestUrl);
                    break;
            }

            var logs = DbManager.Logs.Find(filter).Sort(sort).ToList();
            return new Page<DataAccess.Log>()
            {
                Context = logs,
                CurrentPage = po.PageIndex,
                ItemsPerPage = po.Limit,
                TotalItems = logs.Count,
                TotalPages = (long)Math.Ceiling(logs.Count * 1.0 / po.Limit),
                Items = logs.Skip(po.Offset).Take(po.Limit).ToList()
            };
        }

        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static void DeleteLogAsync() => System.Threading.Tasks.Task.Run(() => DbManager.Logs.DeleteMany(log => log.LogTime < DateTime.Now.AddDays(-7)));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.Log log)
        {
            log.LogTime = DateTime.Now;
            DbManager.Logs.InsertOne(log);
            return true;
        }
    }
}
