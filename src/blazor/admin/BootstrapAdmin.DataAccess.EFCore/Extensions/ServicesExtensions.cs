using BootstrapAdmin.DataAccess.EFCore;
using BootstrapAdmin.DataAccess.EFCore.Services;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEFCoreDataAccessServices(this IServiceCollection services)
        {
            services.AddDbContextFactory<BootstrapAdminContext>((provider, option) =>
            {
                //TODO: 后期改造成自定适配
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connString = configuration.GetConnectionString("bb");
                option.UseSqlite(connString);
            });

            // 增加数据服务
            services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

            services.AddSingleton<INavigations, NavigationsService>();
            services.AddSingleton<IDicts, DictsService>();
            return services;
        }
    }
}
