// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Bootstrap.Admin.Query;
using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Admin.Controllers.Api
{
    /// <summary>
    /// SQL 语句执行日志 webapi
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SQLController : ControllerBase
    {
        /// <summary>
        /// 获取执行日志数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public QueryData<DBLog> Get([FromQuery]QuerySQLOption value)
        {
            return value.RetrieveData();
        }
    }
}
