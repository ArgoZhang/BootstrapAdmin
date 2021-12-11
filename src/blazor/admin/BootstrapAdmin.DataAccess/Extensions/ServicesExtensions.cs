using BootstrapAdmin.DataAccess.Services;
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
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IMenu, MenuService>();
            return services;
        }
    }
}
