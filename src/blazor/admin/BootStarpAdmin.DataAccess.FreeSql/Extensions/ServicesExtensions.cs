using BootStarpAdmin.DataAccess.FreeSql.Extensions;
using BootStarpAdmin.DataAccess.FreeSql.Service;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using FreeSql;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddFreeSql(this IServiceCollection services, Action<IServiceProvider, FreeSqlBuilder> freeSqlBuilder)
    {
        services.TryAddSingleton<IFreeSql>(provider =>
        {
            var builder = new FreeSqlBuilder();
            freeSqlBuilder(provider, builder);
            var instance = builder.Build();
            instance.Mapper();
            return instance;
        });

        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<IUser, UserService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<IRole, RoleService>();
        return services;
    }
}
