// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.HealthChecks;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplication UseBootstrapBlazorAdmin(this WebApplication builder)
    {
        // 开启健康检查
        builder.MapBootstrapHealthChecks();

        builder.UseBootstrapBlazor();

        builder.UseAuthentication();
        builder.UseAuthorization();

        // 激活 ICacheManager
        builder.Services.GetRequiredService<BootstrapAdmin.Caching.ICacheManager>();

        return builder;
    }
}
