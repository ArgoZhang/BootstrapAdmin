using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.ComponentModel;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 后台数据库脚本执行日志实体类
    /// </summary>
    [TableName("DBLogs")]
    public class DBLog
    {

        /// <summary>
        /// 获得/设置 主键ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 获得/设置 当前登陆名
        /// </summary>
        [DisplayName("所属用户")]
        public string? UserName { get; set; }

        /// <summary>
        /// 获得/设置 数据库执行脚本
        /// </summary>
        [DisplayName("脚本内容")]
        public string SQL { get; set; } = "";

        /// <summary>
        /// 获取/设置 用户角色关联状态 checked 标示已经关联 '' 标示未关联
        /// </summary>
        [DisplayName("执行时间")]
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 查询所有SQL日志信息
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual Page<DBLog> RetrievePages(PaginationOption po, DateTime? startTime, DateTime? endTime, string? userName)
        {
            if (string.IsNullOrEmpty(po.Sort)) po.Sort = "LogTime";
            if (string.IsNullOrEmpty(po.Order)) po.Order = "desc";
            var sql = new Sql("select * from DBLogs");
            if (startTime.HasValue) sql.Where("LogTime >= @0", startTime.Value);
            if (endTime.HasValue) sql.Where("LogTime < @0", endTime.Value.AddDays(1).AddSeconds(-1));
            if (startTime == null && endTime == null) sql.Where("LogTime > @0", DateTime.Today.AddMonths(0 - DictHelper.RetrieveExceptionsLogPeriod()));
            if (!string.IsNullOrEmpty(userName)) sql.Where("UserName = @0", userName);
            sql.OrderBy($"{po.Sort} {po.Order}");

            using var db = DbManager.Create();
            return db.Page<DBLog>(po.PageIndex, po.Limit, sql);
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
                DbManager.Create().Execute("delete from DBLogs where LogTime < @0", dtm);
            });
        }

        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Save(DBLog p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            DeleteLogAsync();
            using var db = DbManager.Create(enableLog: false);
            db.Save(p);
            return true;
        }
    }
}
