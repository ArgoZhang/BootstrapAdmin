using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;
using System.Globalization;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    public class QueryLogOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string  OperateType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OperateTimeStart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OperateTimeEnd { get; set; }
        public QueryData<Log> RetrieveData()
        {
            var data = LogHelper.RetrieveLogs(string.Empty);
            if (!string.IsNullOrEmpty(OperateType))
            {
                data = data.Where(t => t.OperationType.ToString().Contains(OperateType));
            }

            if (!string.IsNullOrEmpty(OperateTimeStart))
            {
                DateTime opTimeStart = StringToDateTime(OperateTimeStart);
                if (opTimeStart != null)
                    data = data.Where(t => IsSmallThen(opTimeStart, t.OperationTime));
            }
            if (!string.IsNullOrEmpty(OperateTimeEnd))
            {
                DateTime opTimeEnd = StringToDateTime(OperateTimeEnd);
                if (opTimeEnd != null)
                    data = data.Where(t => IsSmallThen(t.OperationTime, opTimeEnd));
            }

            var ret = new QueryData<Log>();
            ret.total = data.Count();
            // TODO: 通过option.Sort属性判断对那列进行排序，现在统一对名称列排序
            data = Order == "asc" ? data.OrderBy(t => t.OperationType) : data.OrderByDescending(t => t.OperationType);
            ret.rows = data.Skip(Offset).Take(Limit);
            return ret;
        }
        private static DateTime StringToDateTime(string dt_str)
        {
            DateTime dt ;
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
            dt = Convert.ToDateTime(dt_str, dtFormat);
            return dt;
        }
        /// <summary>
        /// 比较两个DateTime
        /// (去掉了毫秒)
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private static bool IsSmallThen(DateTime d1, DateTime d2)
        {
            return new DateTime(d1.Year, d1.Month, d1.Day, d1.Hour, d1.Minute, d1.Second) <=
                new DateTime(d2.Year, d2.Month, d2.Day, d2.Hour, d2.Minute, d2.Second);
        }
    }
}