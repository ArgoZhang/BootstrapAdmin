using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.HealthChecks;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Services.SMS;
using BootstrapAdmin.Web.Services.SMS.Tencent;
using BootstrapAdmin.Web.Utils;

namespace BootstrapAdmin.Web.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// 添加示例后台任务
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddBootstrapBlazorAdmin(this IServiceCollection services)
        {
            services.AddLogging(logging => logging.AddFileLogger().AddCloudLogger().AddDBLogger(ExceptionsHelper.Log));
            services.AddCors();
            services.AddResponseCompression();

            // 增加 健康检查服务
            services.AddAdminHealthChecks();

            // 增加 BootstrapBlazor 组件
            services.AddBootstrapBlazor();

            // 增加手机短信服务
            services.AddSingleton<ISMSProvider, TencentSMSProvider>();

            // 增加认证授权服务
            services.AddBootstrapAdminSecurity<AdminService>();

            // 增加 BootstrapApp 上下文服务
            services.AddScoped<BootstrapAppContext>();

            // 增加 EFCore 数据服务
            //services.AddEFCoreDataAccessServices((provider, option) =>
            //{
            //    var configuration = provider.GetRequiredService<IConfiguration>();
            //    var connString = configuration.GetConnectionString("bb");
            //    option.UseSqlite(connString);
            //});

            // 增加 PetaPoco 数据服务
            //services.AddPetaPocoDataAccessServices((provider, builder) =>
            //{
            //    var configuration = provider.GetRequiredService<IConfiguration>();
            //    var connString = configuration.GetConnectionString("bb");
            //    builder.UsingProvider<SQLiteDatabaseProvider>()
            //           .UsingConnectionString(connString);
            //});

            services.AddFreeSql((provider, builder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connString = configuration.GetConnectionString("bb");
                builder.UseConnectionString(FreeSql.DataType.Sqlite, connString);
#if DEBUG
                //调试sql语句输出
                builder.UseMonitorCommand(cmd => System.Console.WriteLine(cmd.CommandText));
#endif
            });


            // 增加后台任务
            services.AddTaskServices();
            services.AddHostedService<AdminTaskService>();

            return services;
        }
    }
}
