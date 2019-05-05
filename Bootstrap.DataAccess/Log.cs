using Longbow.Web.Mvc;
using PetaPoco;
using System;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class Log : Trace
    {
        /// <summary>
        /// 获得/设置 操作类型
        /// </summary>
        public string CRUD { get; set; }

        /// <summary>
        /// 获得/设置 请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 查询所有操作日志信息
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="opType"></param>
        /// <returns></returns>
        public virtual new Page<Log> Retrieves(PaginationOption po, DateTime? startTime, DateTime? endTime, string opType)
        {
            var sql = new Sql("select CRUD, UserName, LogTime, Ip, Browser, OS, City, RequestUrl, RequestData from Logs");
            if (startTime.HasValue) sql.Append("where LogTime >= @0", startTime.Value);
            if (endTime.HasValue) sql.Append("where LogTime < @0", endTime.Value.AddDays(1).AddSeconds(-1));
            if (startTime == null && endTime == null) sql.Append("where LogTime > @0", DateTime.Today.AddMonths(0 - DictHelper.RetrieveExceptionsLogPeriod()));
            if (!string.IsNullOrEmpty(opType)) sql.Append("where CRUD = @0", opType);
            sql.Append($"order by {po.Sort} {po.Order}");

            return DbManager.Create().Page<Log>(po.PageIndex, po.Limit, sql);
        }

        /// <summary>
        /// 删除日志信息
        /// </summary>
        /// <returns></returns>
        private static void DeleteLogAsync()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                var dtm = DateTime.Now.AddMonths(0 - DictHelper.RetrieveLogsPeriod());
                DbManager.Create().Execute("delete from Logs where LogTime < @0", dtm);
            });
        }

        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Save(Log p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            DeleteLogAsync();
            p.LogTime = DateTime.Now;
            DbManager.Create().Save(p);
            return true;
        }
    }
}
