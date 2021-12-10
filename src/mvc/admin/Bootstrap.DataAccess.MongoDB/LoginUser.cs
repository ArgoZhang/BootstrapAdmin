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
    public class LoginUser : DataAccess.LoginUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool Log(DataAccess.LoginUser user)
        {
            DbManager.LoginUsers.InsertOne(user);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public override Page<DataAccess.LoginUser> RetrieveByPages(PaginationOption po, DateTime? startTime, DateTime? endTime, string? ip)
        {
            var logs = RetrieveAll(startTime, endTime, ip);
            return new Page<DataAccess.LoginUser>()
            {
                Context = logs,
                CurrentPage = po.PageIndex,
                ItemsPerPage = po.Limit,
                TotalItems = logs.Count(),
                TotalPages = (long)Math.Ceiling(logs.Count() * 1.0 / po.Limit),
                Items = logs.Skip(po.Offset).Take(po.Limit).ToList()
            };
        }

        /// <summary>
        /// 获取所有登录数据
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DataAccess.LoginUser> RetrieveAll(DateTime? startTime, DateTime? endTime, string? ip)
        {
            var filterBuilder = Builders<DataAccess.LoginUser>.Filter;
            var filter = filterBuilder.Empty;
            if (startTime.HasValue) filter = filterBuilder.Gte(l => l.LoginTime, startTime.Value);
            if (endTime.HasValue) filter = filterBuilder.Lt(l => l.LoginTime, endTime.Value.AddDays(1));
            if (!string.IsNullOrEmpty(ip)) filter = filterBuilder.Eq(l => l.Ip, ip);

            return DbManager.LoginUsers
                 .Find(filter)
                 .Sort(Builders<DataAccess.LoginUser>.Sort.Ascending(t => t.LoginTime)).ToList();
        }
    }
}
