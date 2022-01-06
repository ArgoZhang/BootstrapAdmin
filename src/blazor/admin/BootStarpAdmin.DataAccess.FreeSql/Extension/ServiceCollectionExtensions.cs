using BootStarpAdmin.DataAccess.FreeSql.Service;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFreeSql(this IServiceCollection services)
    {
        IFreeSql fsql = new FreeSqlBuilder()
            .UseConnectionString(DataType.Sqlite, @"Data Source=BootstrapAdmin.db")
            .Build();
        services.AddSingleton(fsql);
        services.AddService();
        return services;
    }

    private static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<IUser, UserService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IDict, DictService>();
        return services;
    }
}
