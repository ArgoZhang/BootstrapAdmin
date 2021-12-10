using Longbow.Web.Mvc;
using MongoDB.Driver;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Bootstrap.DataAccess.MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class Exceptions : DataAccess.Exceptions
    {
        private static void ClearExceptions()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                DbManager.Exceptions.DeleteMany(ex => ex.LogTime < DateTime.Now.AddMonths(0 - DictHelper.RetrieveExceptionsLogPeriod()));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Exceptions> Retrieves()
        {
            return DbManager.Exceptions.Find(ex => ex.LogTime >= DateTime.Today.AddMonths(0 - DictHelper.RetrieveExceptionsLogPeriod())).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public override bool Log(Exception ex, NameValueCollection additionalInfo)
        {
            var excep = new DataAccess.Exceptions
            {
                Id = null,
                AppDomainName = AppDomain.CurrentDomain.FriendlyName,
                ErrorPage = additionalInfo?["ErrorPage"] ?? (ex.GetType().Name.Length > 50 ? ex.GetType().Name.Substring(0, 50) : ex.GetType().Name),
                ExceptionType = ex.GetType().FullName,
                LogTime = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                UserId = additionalInfo?["UserId"],
                UserIp = additionalInfo?["UserIp"]
            };
            DbManager.Exceptions.InsertOne(excep);
            ClearExceptions();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public override Page<DataAccess.Exceptions> RetrievePages(PaginationOption po, DateTime? startTime, DateTime? endTime)
        {
            // filter
            var filterBuilder = Builders<DataAccess.Exceptions>.Filter;
            var filter = filterBuilder.Empty;
            if (startTime.HasValue) filter = filterBuilder.Gt("LogTime", startTime.Value);
            if (endTime.HasValue) filter = filterBuilder.Lt("LogTime", endTime.Value.AddDays(1).AddSeconds(-1));
            if (startTime == null && endTime == null) filter = filterBuilder.Gt("LogTime", DateTime.Today.AddMonths(0 - DictHelper.RetrieveExceptionsLogPeriod()));

            // sort
            var sortBuilder = Builders<DataAccess.Exceptions>.Sort;
            var sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.LogTime) : sortBuilder.Descending(t => t.LogTime);
            switch (po.Sort)
            {
                case "ErrorPage":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.ErrorPage) : sortBuilder.Descending(t => t.ErrorPage);
                    break;
                case "UserId":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.UserId) : sortBuilder.Descending(t => t.UserId);
                    break;
                case "UserIp":
                    sort = po.Order == "asc" ? sortBuilder.Ascending(t => t.UserIp) : sortBuilder.Descending(t => t.UserIp);
                    break;
            }

            var exceps = DbManager.Exceptions.Find(filter).Sort(sort).ToList();
            return new Page<DataAccess.Exceptions>()
            {
                Context = exceps,
                CurrentPage = po.PageIndex,
                ItemsPerPage = po.Limit,
                TotalItems = exceps.Count,
                TotalPages = (long)Math.Ceiling(exceps.Count * 1.0 / po.Limit),
                Items = exceps.Skip(po.Offset).Take(po.Limit).ToList()
            };
        }
    }
}
