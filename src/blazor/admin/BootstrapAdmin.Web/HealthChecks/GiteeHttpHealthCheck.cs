// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace BootstrapAdmin.Web.HealthChecks;

/// <summary>
/// Gitee 接口检查器
/// </summary>
class GiteeHttpHealthCheck : IHealthCheck
{
    private GiteeHttpClient Client { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="client"></param>
    public GiteeHttpHealthCheck(GiteeHttpClient client)
    {
        Client = client;
    }

    /// <summary>
    /// 异步检查方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var urls = new string[] { "Issues", "Pulls", "Releases", "Builds" };
        var data = new Dictionary<string, object>();

        Task.WaitAll(urls.Select(url => Task.Run(async () =>
        {
            var sw = Stopwatch.StartNew();
            Exception? error = null;
            object? result = null;
            try
            {
                result = await Client.HttpClient.GetFromJsonAsync<object>($"/api/Gitee/{url}", cancellationToken);
            }
            catch (Exception ex) { error = ex; }
            sw.Stop();
            data.Add(url, error == null
                ? $"{result} Elapsed: {sw.Elapsed}"
                : $"Elapsed: {sw.Elapsed} Exception: {error}");
        })).ToArray(), cancellationToken);
        return Task.FromResult(HealthCheckResult.Healthy("Ok", data));
    }
}
