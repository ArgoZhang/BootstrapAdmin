using BootstrapAdmin.DataAccess.PetaPoco;
using BootstrapAdmin.DataAccess.PetaPoco.Services;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using BootstrapBlazor.DataAcces.PetaPoco.Services;
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
        public static IServiceCollection AddPetaPocoDataAccessServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IDatabase>(provider =>
            {
                //TODO: 后期改造成自定适配
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connString = configuration.GetConnectionString("bb");
                return new Database<SQLiteDatabaseProvider>(connString, new BootstrapAdminConventionMapper());
            });

            // 增加数据服务
            services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

            // 增加业务服务
            services.AddSingleton<INavigation, NavigationService>();
            services.AddSingleton<IDict, DictService>();
            services.AddSingleton<IUser, UserService>();
            services.AddSingleton<ILogin, LoginService>();
            services.AddSingleton<IRole, RoleService>();
            services.AddSingleton<IGroup, GroupService>();
            services.AddSingleton<IApp, AppService>();
            return services;
        }
    }
}
