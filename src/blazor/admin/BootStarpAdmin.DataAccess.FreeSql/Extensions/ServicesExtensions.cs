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

        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

        // 增加业务服务
        services.AddSingleton<IApp, AppService>();
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IException, ExceptionService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IRole, RoleService>();
        services.AddSingleton<IUser, UserService>();
        return services;
    }
}
