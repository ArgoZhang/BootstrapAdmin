// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.HealthChecks;

namespace BootstrapAdmin.Web.HealthChecks;

/// <summary>
/// 健康检查扩展类
/// </summary>
static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// 添加 BootstrapAdmin 健康检查
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAdminHealthChecks(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient<GiteeHttpClient>();

        var builder = services.AddHealthChecks();
        builder.AddCheck<DBHealthCheck>("db");
        builder.AddBootstrapAdminHealthChecks();
        builder.AddCheck<GiteeHttpHealthCheck>("Gitee");
        return services;
    }
}
