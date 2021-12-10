using Longbow.Web.Mvc;
using MongoDB.Driver;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Trace : DataAccess.Trace
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Page<DataAccess.Trace> RetrievePages(PaginationOption po, DateTime? startTime, DateTime? endTime, string? ip)
        {
            // filter
            var filterBuilder = Builders<DataAccess.Trace>.Filter;
            var filter = filterBuilder.Empty;
            if (startTime.HasValue) filter = filterBuilder.Gt("LogTime", startTime.Value);
            if (endTime.HasValue) filter = filterBuilder.Lt("LogTime", endTime.Value.AddDays(1).AddSeconds(-1));
            if (!string.IsNullOrEmpty(ip)) filter = filterBuilder.Eq("Ip", ip);
            if (startTime == null && endTime == null) filter = filterBuilder.Gt("LogTime", DateTime.Today.AddMonths(0 - DictHelper.RetrieveAccessLogPeriod()));

            // sort
            var sortBuilder = Builders<DataAccess.Trace>.Sort;
            var sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.LogTime) : sortBuilder.Descending(t => t.LogTime);
            switch (po.Sort)
            {
                case "IP":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.Ip) : sortBuilder.Descending(t => t.Ip);
                    break;
                case "UserName":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.UserName) : sortBuilder.Descending(t => t.UserName);
                    break;
                case "City":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.City) : sortBuilder.Descending(t => t.City);
                    break;
                case "Browser":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.Browser) : sortBuilder.Descending(t => t.Browser);
                    break;
                case "OS":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.OS) : sortBuilder.Descending(t => t.OS);
                    break;
                case "RequestUrl":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.RequestUrl) : sortBuilder.Descending(t => t.RequestUrl);
                    break;
            }

            var traces = DbManager.Traces.Find(filter).Sort(sort).ToList();
            return new Page<DataAccess.Trace>()
            {
                Context = traces,
                CurrentPage = po.PageIndex,
                ItemsPerPage = po.Limit,
                TotalItems = traces.Count,
                TotalPages = (long)Math.Ceiling(traces.Count * 1.0 / po.Limit),
                Items = traces.Skip(po.Offset).Take(po.Limit).ToList()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Trace> RetrieveAll(DateTime? startTime, DateTime? endTime, string? ip)
        {
            var filterBuilder = Builders<DataAccess.Trace>.Filter;
            var filter = filterBuilder.Empty;
            if (startTime.HasValue) filter = filterBuilder.Gt("LogTime", startTime.Value);
            if (endTime.HasValue) filter = filterBuilder.Lt("LogTime", endTime.Value.AddDays(1).AddSeconds(-1));
            if (!string.IsNullOrEmpty(ip)) filter = filterBuilder.Eq("Ip", ip);

            // sort
            var sort = Builders<DataAccess.Trace>.Sort.Ascending(t => t.LogTime);
            return DbManager.Traces.Find(filter).Sort(sort).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Save(DataAccess.Trace p)
        {
            DbManager.Traces.InsertOne(p);
            ClearTraces();
            return true;
        }

        private static void ClearTraces()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                DbManager.Traces.DeleteMany(t => t.LogTime < DateTime.Now.AddMonths(0 - DictHelper.RetrieveAccessLogPeriod()));
            });
        }
    }
}
