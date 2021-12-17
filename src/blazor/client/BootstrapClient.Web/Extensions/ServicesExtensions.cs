using BootstrapAdmin.Web.Services;
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
        public static IServiceCollection AddBootstrapBlazorClient(this IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddCors();
            services.AddResponseCompression();

            // 增加 BootstrapBlazor 组件
            services.AddBootstrapBlazor();

            // 增加认证授权服务
            services.AddBootstrapAdminSecurity<AdminService>();

            // 增加 PetaPoco 数据服务
            services.AddPetaPocoDataAccessServices();

            return services;
        }
    }
}
