// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 登录日志查询条件 
    /// </summary>
    public class QueryLoginOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string? LoginIP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<LoginUser> RetrieveData()
        {
            var data = LoginHelper.RetrievePages(this, StartTime, EndTime, LoginIP);
            return new QueryData<LoginUser>
            {
                total = data.TotalItems,
                rows = data.Items
            };
        }
    }
}
