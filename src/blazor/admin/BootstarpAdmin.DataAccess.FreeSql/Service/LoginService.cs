// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.FreeSql.Service;

class LoginService : ILogin
{
    private IFreeSql FreeSql { get; }

    public LoginService(IFreeSql freeSql) => FreeSql = freeSql;

    public bool Log(string userName, string? IP, string? OS, string? browser, string? address, string? userAgent, bool result)
    {
        var loginUser = new LoginLog()
        {
            UserName = userName,
            LoginTime = DateTime.Now,
            Ip = IP,
            City = address,
            OS = OS,
            Browser = browser,
            UserAgent = userAgent,
            Result = result ? "登录成功" : "登录失败"
        };
        FreeSql.Insert(loginUser).ExecuteAffrows();
        return true;
    }
}
