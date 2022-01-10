﻿namespace Microsoft.AspNetCore.Builder;

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

        builder.UseAuthentication();
        builder.UseAuthorization();

        return builder;
    }
}
