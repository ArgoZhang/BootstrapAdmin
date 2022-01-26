// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// SQL执行查询配置类
    /// </summary>
    public class QuerySQLOption : PaginationOption
    {
        /// <summary>
        /// 获得/设置 用户登录名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 获得/设置 开始时间
        /// </summary>
        public DateTime? OperateTimeStart { get; set; }

        /// <summary>
        /// 获得/设置 结束时间
        /// </summary>
        public DateTime? OperateTimeEnd { get; set; }

        /// <summary>
        /// 查询数据方法
        /// </summary>
        /// <returns></returns>
        public QueryData<DBLog> RetrieveData()
        {
            var data = LogHelper.RetrieveDBLogs(this, OperateTimeStart, OperateTimeEnd, UserName);
            var ret = new QueryData<DBLog>();
            ret.total = data.TotalItems;
            ret.rows = data.Items;
            return ret;
        }
    }
}
