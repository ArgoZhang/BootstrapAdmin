using Longbow.Tasks;
using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

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
        public static Page<Log> RetrievePages(PaginationOption op, DateTime? startTime, DateTime? endTime, string opType) => DbContextManager.Create<Log>().RetrievePages(op, startTime, endTime, opType);

        /// <summary>
        /// 查询所有日志信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Log> RetrieveAll(DateTime? startTime, DateTime? endTime, string opType) => DbContextManager.Create<Log>().RetrieveAll(startTime, endTime, opType);

        /// <summary>
        /// 保存新增的日志信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool Save(Log log)
        {
            log.LogTime = DateTime.Now;
            return DbContextManager.Create<Log>().Save(log);
        }

        #region 数据库脚本执行日志相关代码
        private static BlockingCollection<DBLog> _messageQueue = new BlockingCollection<DBLog>(new ConcurrentQueue<DBLog>());
        /// <summary>
        /// 添加数据库日志实体类到内部集合中
        /// </summary>
        /// <param name="log"></param>
        public static System.Threading.Tasks.Task AddDBLog(DBLog log) => System.Threading.Tasks.Task.Run(() =>
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                _messageQueue.Add(log);
            }
        });

        /// <summary>
        /// 数据库脚本执行日志任务实体类
        /// </summary>
        public class DbLogTask : ITask
        {
            /// <summary>
            /// 任务执行方法
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public System.Threading.Tasks.Task Execute(CancellationToken cancellationToken)
            {
                var logs = new List<DBLog>();
                while (_messageQueue.TryTake(out var log))
                {
                    logs.Add(log);
                };
                using (var db = DbManager.Create(enableLog: false))
                {
                    db.InsertBatch(logs);
                }
                return System.Threading.Tasks.Task.CompletedTask;
            }
        }
        #endregion
    }
}
