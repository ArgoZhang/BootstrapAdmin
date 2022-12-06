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

    private IHttpClientFactory HttpClientFactory { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public CloudLoggerProvider(IOptionsMonitor<CloudLoggerOption> options, IHttpClientFactory httpClientFactory) : base(new Func<string, LogLevel, bool>((name, logLevel) => logLevel >= LogLevel.Error))
    {
        Options = options;
        HttpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public override ILogger CreateLogger(string categoryName) => new CloudLogger(categoryName, Options, HttpClientFactory, Filter, null, Configuration);
}

class CloudLogger : LoggerBase
{
    private IOptionsMonitor<CloudLoggerOption> Options { get; }

    private IHttpClientFactory HttpClientFactory { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    /// <param name="httpClientFactory"></param>
    /// <param name="filter"></param>
    /// <param name="scopeProvider"></param>
    /// <param name="config"></param>
    public CloudLogger(string name, IOptionsMonitor<CloudLoggerOption> options, IHttpClientFactory httpClientFactory, Func<string, LogLevel, bool>? filter, IExternalScopeProvider? scopeProvider, IConfiguration? config) : base(name, filter, scopeProvider, config)
    {
        Options = options;
        HttpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="content"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected override void WriteMessageCore(string content)
    {
        var url = Options.CurrentValue.Url;
        if (!string.IsNullOrEmpty(url))
        {
            var client = HttpClientFactory.CreateClient(Options.CurrentValue.Url);
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Connection.Add("keep-alive");

            try
            {
                Task.Run(() => client.PostAsJsonAsync(url, content));
            }
            catch { }
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
    public string Url { get; set; } = "";
}
