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
        /// <returns></returns>
        public override Page<DataAccess.LoginUser> Retrieves(PaginationOption po)
        {
            var logs = DbManager.LoginUsers
                .Find(Builders<DataAccess.LoginUser>.Filter.Empty)
                .Sort(Builders<DataAccess.LoginUser>.Sort.Descending(t => t.LoginTime))
                .ToList();

            return new Page<DataAccess.LoginUser>()
            {
                Context = logs,
                CurrentPage = po.PageIndex,
                ItemsPerPage = po.Limit,
                TotalItems = logs.Count,
                TotalPages = (long)Math.Ceiling(logs.Count * 1.0 / po.Limit),
                Items = logs.Skip(po.Offset).Take(po.Limit).ToList()
            };
        }
    }
}
