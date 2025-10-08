// Copyright (c) Argo Zhang (argo@live.ca). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://pro.blazor.zone

using Microsoft.AspNetCore.Mvc;

namespace BootstrapAdmin.Web.Controllers;

/// <summary>
/// 
/// </summary>
public class AccountController : Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return Ok();
    }
}
