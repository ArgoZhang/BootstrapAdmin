using BootstrapAdmin.DataAccess.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetaPoco;
using PetaPoco.Providers;

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
            services.TryAddSingleton<IDatabase>(provider =>
            {
                //TODO: 后期改造成自定适配
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connString = configuration.GetConnectionString("bb");
                return new Database<SQLiteDatabaseProvider>(connString);
            });

            services.TryAddSingleton<INavigations, NavigationsService>();
            services.TryAddSingleton<IDicts, DictsService>();
            return services;
        }
    }
}
