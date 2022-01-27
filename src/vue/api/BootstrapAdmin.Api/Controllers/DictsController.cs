// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BootstrapAdmin.Api.Controllers
{
    /// <summary>
    /// 字典表维护控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DictsController : ControllerBase
    {
        private IDict DictService { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictService"></param>
        public DictsController(IDict dictService) => DictService = dictService;

        /// <summary>
        /// 获取所有字典表数据方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Dict>> Get()
        {
            return DictService.GetAll();
        }
        /// <summary>
        /// 保存字典方法
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody] Dict value)
        {
            return true;
        }

        /// <summary>
        /// 删除字典项方法
        /// </summary>
        /// <param name="value"></param>
        [HttpDelete]
        [Authorize(Roles = "Administrators")]
        public bool Delete([FromBody] IEnumerable<string> value)
        {
            return true;
        }
    }
}
