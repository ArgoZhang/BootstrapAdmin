using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Services.SMS;
using BootstrapAdmin.Web.Services.SMS.Tencent;
using BootstrapAdmin.Web.Utils;
//using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddLogging(logging => logging.AddFileLogger().AddCloudLogger().AddDBLogger(ExceptionsHelper.Log));
            services.AddCors();
            services.AddResponseCompression();

            // 增加 BootstrapBlazor 组件
            services.AddBootstrapBlazor();

            // 增加手机短信服务
            services.AddScoped<ISMSProvider, TencentSMSProvider>();

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
            services.AddPetaPocoDataAccessServices();

            // 增加后台任务
            services.AddTaskServices();
            services.AddHostedService<AdminTaskService>();

            return services;
        }
    }
}
