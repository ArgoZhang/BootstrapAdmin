using Bootstrap.Client.Controllers.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Bootstrap.Client.Tasks
{
    /// <summary>
    /// 发布任务管理操作类
    /// </summary>
    public static class DeployTaskManager
    {
        private static BlockingCollection<GiteePushEventArgs> _pool = new BlockingCollection<GiteePushEventArgs>(new ConcurrentQueue<GiteePushEventArgs>());
        private static IServiceCollection _services;

        /// <summary>
        /// IServiceCollection 实例
        /// </summary>
        /// <param name="services"></param>
        internal static void RegisterServices(IServiceCollection services) => _services = services;

        /// <summary>
        /// 添加自动发布任务到队列中
        /// </summary>
        /// <param name="args"></param>
        public static void Add(GiteePushEventArgs args)
        {
            // 判断是否需要自动发布
            var sp = _services.BuildServiceProvider();
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<DeployController>>();
            var option = config.GetSection<DeployOptions>().Get<DeployOptions>();
            if (option.Enabled && !string.IsNullOrEmpty(option.DeployFile))
            {
                if (!_pool.IsAddingCompleted)
                {
                    _pool.Add(args);
                }

                RunAsync(logger, option).ConfigureAwait(false);
            }
        }

        private static bool _running;
        private static object _locker = new object();

        private static async Task RunAsync(ILogger<DeployController> logger, DeployOptions options)
        {
            if (!_running)
            {
                await Task.Run(() =>
                {
                    // 线程等待防止多个部署任务同时执行
                    lock (_locker)
                    {
                        if (!_running)
                        {
                            _running = true;
                            while (_pool.TryTake(out var args))
                            {
                                // 分析提交分支
                                if (args.Ref.SpanSplit("/").LastOrDefault().Equals(options.Branch, StringComparison.OrdinalIgnoreCase) && CanDeploy(options.OSPlatform))
                                {
                                    // 仅部署配置分支代码
                                    Deploy(logger, options.DeployFile);
                                }
                            }
                        }
                    }
                    _running = false;
                });
            }
        }

        private static bool CanDeploy(string osPlatform)
        {
            var os = OSPlatform.Create(osPlatform);
            return !RuntimeInformation.IsOSPlatform(os);
        }

        private static void Deploy(ILogger<DeployController> logger, string deployFile)
        {
            // 调用部署脚本
            try
            {
                //var psi = new ProcessStartInfo("sh", "~/BootstrapAdmin/deploy-admin.sh");
                var cmd = deployFile;
                if (File.Exists(cmd))
                {
                    var psi = new ProcessStartInfo("sh", $"\"{cmd}\"");
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    var p = Process.Start(psi);
                    p.WaitForExit();
                    var result = p.StandardOutput.ReadToEnd();
                    logger.LogError("deploy success: {0}", result);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
            }
        }
    }
}
