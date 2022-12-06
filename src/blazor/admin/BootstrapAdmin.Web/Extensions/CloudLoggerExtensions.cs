// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 邮件日志扩展方法
/// </summary>
static class CloudLoggerExtensions
{
    /// <summary>
    /// 注册邮件日志方法
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddCloudLogger(this ILoggingBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<IConfigureOptions<CloudLoggerOption>, LoggerProviderConfigureOptions<CloudLoggerOption, CloudLoggerProvider>>();
        builder.Services.AddSingleton<IOptionsChangeTokenSource<CloudLoggerOption>, LoggerProviderOptionsChangeTokenSource<CloudLoggerOption, CloudLoggerProvider>>();
        builder.Services.AddSingleton<ILoggerProvider, CloudLoggerProvider>();
        return builder;
    }
}

/// <summary>
/// 云日志提供类
/// </summary>
[ProviderAlias("Cloud")]
class CloudLoggerProvider : LoggerProvider
{
    private IOptionsMonitor<CloudLoggerOption> Options { get; }

    private IServiceProvider Provider { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public CloudLoggerProvider(IOptionsMonitor<CloudLoggerOption> options, IServiceProvider provider) : base(new Func<string, LogLevel, bool>((name, logLevel) => logLevel >= LogLevel.Error))
    {
        Options = options;
        Provider = provider;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public override ILogger CreateLogger(string categoryName) => new CloudLogger(categoryName, Options, Provider, Filter, null, Configuration);
}

class CloudLogger : LoggerBase
{
    private IOptionsMonitor<CloudLoggerOption> Options { get; }

    private IServiceProvider Provider { get; }

    private IHttpClientFactory? HttpClientFactory { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    /// <param name="provider"></param>
    /// <param name="filter"></param>
    /// <param name="scopeProvider"></param>
    /// <param name="config"></param>
    public CloudLogger(string name, IOptionsMonitor<CloudLoggerOption> options, IServiceProvider provider, Func<string, LogLevel, bool>? filter, IExternalScopeProvider? scopeProvider, IConfiguration? config) : base(name, filter, scopeProvider, config)
    {
        Options = options;
        Provider = provider;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="content"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected override void WriteMessageCore(string content)
    {
        var url = Options.CurrentValue.Url;
        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(content))
        {
            try
            {
                Task.Run(() => CreateHttpClient().PostAsJsonAsync(url, content));
            }
            catch { }
        }
    }

    private HttpClient? httpClient;

    private HttpClient CreateHttpClient()
    {
        return httpClient ?? Create();

        HttpClient Create()
        {
            HttpClientFactory ??= Provider.GetRequiredService<IHttpClientFactory>();
            httpClient ??= HttpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            return httpClient;
        }
    }
}

/// <summary>
/// 云日志配置类
/// </summary>
class CloudLoggerOption
{
    /// <summary>
    /// 获得/设置 云日志地址
    /// </summary>
    public string? Url { get; set; }
}
