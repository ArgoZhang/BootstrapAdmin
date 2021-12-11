using BootstrapAdmin.DataAccess.EFCore;
using BootstrapAdmin.DataAccess.EFCore.Services;
using BootstrapAdmin.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

            services.TryAddSingleton<INavigations, NavigationsService>();
            services.TryAddSingleton<IDicts, DictsService>();
            return services;
        }
    }
}
