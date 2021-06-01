using Longbow.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 邮件日志扩展方法
    /// </summary>
    public static class CloudLoggerExtensions
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
    public class CloudLoggerProvider : LoggerProvider
    {
        private readonly HttpClient httpClient;
        private readonly IDisposable optionsReloadToken;
        private CloudLoggerOption option;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CloudLoggerProvider(IOptionsMonitor<CloudLoggerOption> options) : base(null, new Func<string, LogLevel, bool>((name, logLevel) => logLevel >= LogLevel.Error))
        {
            optionsReloadToken = options.OnChange(op => option = op);
            option = options.CurrentValue;

            httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");

            LogCallback = new Action<string>(async message =>
            {
                if (!string.IsNullOrEmpty(option.Url))
                {
                    try { await httpClient.PostAsJsonAsync(option.Url, message).ConfigureAwait(false); }
                    catch { }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                httpClient.Dispose();
                optionsReloadToken.Dispose();
            }
        }
    }

    /// <summary>
    /// 云日志配置类
    /// </summary>
    public class CloudLoggerOption
    {
        /// <summary>
        /// 获得/设置 云日志地址
        /// </summary>
        public string Url { get; set; } = "";
    }
}
