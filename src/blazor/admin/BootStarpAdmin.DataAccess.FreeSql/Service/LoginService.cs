// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;

namespace BootStarpAdmin.DataAccess.FreeSql.Service;

class LoginService : ILogin
{
    public Task<bool> Log(string userName, bool result)
    {
        return Task.FromResult(true);
    }
}
