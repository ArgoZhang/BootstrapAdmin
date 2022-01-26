// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.HealthChecks;

/// <summary>
/// Gitee HttpClient 操作类
/// </summary>
public class GiteeHttpClient
{
    /// <summary>
    /// HttpClient 实例
    /// </summary>
    public HttpClient HttpClient { get; private set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="client"></param>
    /// <param name="accessor"></param>
    public GiteeHttpClient(HttpClient client, IHttpContextAccessor accessor)
    {
        client.Timeout = TimeSpan.FromSeconds(10);
        client.DefaultRequestHeaders.Connection.Add("keep-alive");
        if (accessor.HttpContext != null)
        {
            client.BaseAddress = new Uri($"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host}{accessor.HttpContext?.Request.PathBase}");
        }
        HttpClient = client;
    }
}
