using Longbow;
using Longbow.Configuration;
using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Exceptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppDomainName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 获得/设置 时间描述 2分钟内为刚刚
        /// </summary>
        public string Period { get; set; }

        private static void ClearExceptions() => System.Threading.Tasks.Task.Run(() =>
        {
            DbManager.Create().Execute("delete from Exceptions where LogTime < @0", DateTime.Now.AddMonths(0 - LgbConvert.ReadValue(ConfigurationManager.AppSettings["KeepExceptionsPeriod"], 1)));
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="additionalInfo"></param>
        /// <returns></returns>
        public virtual bool Log(Exception ex, NameValueCollection additionalInfo)
        {
            if (ex == null) return true;

            var errorPage = additionalInfo?["ErrorPage"] ?? (ex.GetType().Name.Length > 50 ? ex.GetType().Name.Substring(0, 50) : ex.GetType().Name);
            DbManager.Create().Insert(new Exceptions()
            {
                AppDomainName = AppDomain.CurrentDomain.FriendlyName,
                ErrorPage = errorPage,
                UserId = additionalInfo?["UserId"],
                UserIp = additionalInfo?["UserIp"],
                ExceptionType = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                LogTime = DateTime.Now
            });
            ClearExceptions();
            return true;
        }

        /// <summary>
        /// 查询一周内所有异常
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Exceptions> Retrieves() => DbManager.Create().Fetch<Exceptions>("select * from Exceptions where LogTime > @0 order by LogTime desc", DateTime.Now.AddDays(-7));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public virtual Page<Exceptions> RetrievePages(PaginationOption po, DateTime? startTime, DateTime? endTime)
        {
            var sql = new Sql("select * from Exceptions");
            if (startTime.HasValue) sql.Append("where LogTime > @0", startTime.Value);
            if (endTime.HasValue) sql.Append("where LogTime < @0", endTime.Value);
            if (startTime == null && endTime == null) sql.Append("where LogTime > @0", DateTime.Today.AddDays(-7));
            sql.Append("order by @0", $"{po.Sort} {po.Order}");

            return DbManager.Create().Page<Exceptions>(po.PageIndex, po.Limit, sql);
        }
    }
}
