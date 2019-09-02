using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 用户访问数据实体类
    /// </summary>
    [TableName("Traces")]
    public class Trace
    {
        /// <summary>
        /// 获得/设置 操作日志主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获得/设置 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获得/设置 操作时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 获得/设置 客户端IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 获得/设置 客户端地点
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 获得/设置 客户端浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 获得/设置 客户端操作系统
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// 获取/设置 请求网址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 获得/设置 客户端 UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 保存用户访问数据历史记录
        /// </summary>
        /// <param name="p"></param>
        public virtual bool Save(Trace p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            DbManager.Create().Save(p);
            ClearTraces();
            return true;
        }

        /// <summary>
        /// 查询用户访问分页数据
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public virtual Page<Trace> RetrievePages(PaginationOption po, DateTime? startTime, DateTime? endTime, string ip)
        {
            var sql = new Sql("select * from Traces");
            if (startTime.HasValue) sql.Where("LogTime > @0", startTime.Value);
            if (endTime.HasValue) sql.Where("LogTime < @0", endTime.Value.AddDays(1).AddSeconds(-1));
            if (startTime == null && endTime == null) sql.Where("LogTime > @0", DateTime.Today.AddMonths(0 - DictHelper.RetrieveAccessLogPeriod()));
            if (!string.IsNullOrEmpty(ip)) sql.Where("IP = @0", ip);
            sql.OrderBy($"{po.Sort} {po.Order}");

            return DbManager.Create().Page<Trace>(po.PageIndex, po.Limit, sql);
        }

        /// <summary>
        /// 查询所有用户数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public virtual IEnumerable<Trace> RetrieveAll(DateTime? startTime, DateTime? endTime, string ip)
        {
            var sql = new Sql("select UserName, LogTime, IP, Browser, OS, City, RequestUrl from Traces");
            if (startTime.HasValue) sql.Where("LogTime > @0", startTime.Value);
            if (endTime.HasValue) sql.Where("LogTime < @0", endTime.Value.AddDays(1).AddSeconds(-1));
            if (!string.IsNullOrEmpty(ip)) sql.Where("IP = @0", ip);
            sql.OrderBy("LogTime");

            return DbManager.Create().Fetch<Trace>(sql);
        }

        private static void ClearTraces() => System.Threading.Tasks.Task.Run(() =>
        {
            DbManager.Create().Execute("delete from Traces where LogTime < @0", DateTime.Now.AddMonths(0 - DictHelper.RetrieveAccessLogPeriod()));
        });
    }
}
