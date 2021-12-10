using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 操作日志相关操作类
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <param name="op"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="opType"></param>
        /// <returns></returns>
        public static Page<Log> RetrievePages(PaginationOption op, DateTime? startTime, DateTime? endTime, string? opType) => DbContextManager.Create<Log>()?.RetrievePages(op, startTime, endTime, opType) ?? new Page<Log>() { Items = new List<Log>() };

        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Log> RetrieveAll(DateTime? startTime, DateTime? endTime, string? opType) => DbContextManager.Create<Log>()?.RetrieveAll(startTime, endTime, opType) ?? new Log[0];

        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool Save(Log log)
        {
            log.LogTime = DateTime.Now;
            return DbContextManager.Create<Log>()?.Save(log) ?? false;
        }

        /// <summary>
        /// 查询所有SQL日志信息
        /// </summary>
        /// <param name="op"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Page<DBLog> RetrieveDBLogs(PaginationOption op, DateTime? startTime, DateTime? endTime, string? userName) => DbContextManager.Create<DBLog>()?.RetrievePages(op, startTime, endTime, userName) ?? new Page<DBLog>() { Items = new List<DBLog>() };
    }
}
