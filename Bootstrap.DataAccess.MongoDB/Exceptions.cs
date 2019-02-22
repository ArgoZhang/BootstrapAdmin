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
                DbManager.Exceptions.DeleteMany(ex => ex.LogTime < DateTime.Now.AddDays(-7));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.Exceptions> Retrieves()
        {
            return DbManager.Exceptions.Find(ex => ex.LogTime >= DateTime.Now.AddDays(-7)).ToList();
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
            var exceps = DbManager.Exceptions.Find(FilterDefinition<DataAccess.Exceptions>.Empty).ToList();
            // sort
            var orderProxy = po.Order == "asc" ?
                new Func<Func<DataAccess.Exceptions, string>, List<DataAccess.Exceptions>>(p => exceps.OrderBy(p).ToList()) :
                new Func<Func<DataAccess.Exceptions, string>, List<DataAccess.Exceptions>>(p => exceps.OrderByDescending(p).ToList());

            var logTimeProxy = po.Order == "asc" ?
                new Func<Func<DataAccess.Exceptions, DateTime>, List<DataAccess.Exceptions>>(p => exceps.OrderBy(p).ToList()) :
                new Func<Func<DataAccess.Exceptions, DateTime>, List<DataAccess.Exceptions>>(p => exceps.OrderByDescending(p).ToList());

            switch (po.Sort)
            {
                case "ErrorPage":
                    exceps = orderProxy(ex => ex.ErrorPage);
                    break;
                case "UserId":
                    exceps = orderProxy(ex => ex.UserId);
                    break;
                case "UserIp":
                    exceps = orderProxy(ex => ex.UserIp);
                    break;
                case "LogTime":
                    exceps = logTimeProxy(ex => ex.LogTime);
                    break;
            }
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
